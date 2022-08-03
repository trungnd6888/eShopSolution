using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using MailKit.Net.Smtp;

namespace eShopSolution.Application.System.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<AppUser> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<Hashtable> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return new Hashtable()
            {
                {"error", "Username or password invalid" }
            };

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (result.IsLockedOut) return new Hashtable()
            {
                { "error", "Username is locked" }
            };

            if (!result.Succeeded) return new Hashtable()
            {
                { "error", "Username or password invalid" }
            };

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, String.Join(";", roles) ),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new Hashtable()
            {
                {"token", new JwtSecurityTokenHandler().WriteToken(token)}
            };
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded ? true : false;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordRequest request, string url)
        {
            // find user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) return false;

            //get token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //send email
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("destinee.koelpin99@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.Email));

            email.Subject = "Send mail from Asp.net Core";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<a href='" + url + token + "'>Click here reset password: </a>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("destinee.koelpin99@ethereal.email", "A9HSNskdgxg6UQMEFH");
            smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }

        public async Task<Hashtable> ResetPassword(ResetPasswordRequest request, string token)
        {
            //find user
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new Hashtable()
            {
                {"error", "Email is invalid" },
            };

            //change password
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded) return new Hashtable()
            {
                { "error", result.Errors},
            };

            return new Hashtable()
            {
                { "result", true }
            };
        }
    }
}