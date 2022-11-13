using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Application.Common.Mail;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.Application.System.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        private readonly IMailService _mailService;
        private readonly EShopDbContext _context;

        public AuthService(EShopDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config, IStorageService storageService, IMailService mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _storageService = storageService;
            _mailService = mailService;
            _context = context;
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

            var roleNames = await _userManager.GetRolesAsync(user);

            var roles = new List<AppRole>();
            if (roleNames != null && roleNames.Count > 0)
            {
                foreach (var item in roleNames)
                {
                    roles.Add(await _roleManager.FindByNameAsync(item));
                }
            }

            var roleClaims = new List<Claim>();
            if (roles != null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    var claimOfRoles = await _roleManager.GetClaimsAsync(role);

                    claimOfRoles.ToList().ForEach(x => roleClaims.Add(x));
                }
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Email, user.Email != null ? user.Email : string.Empty),
                new Claim(ClaimTypes.Uri, user.AvatarImage != null ? _storageService.GetFileUrl( user.AvatarImage) : string.Empty),
            };

            if (roleNames != null && roleNames.Count > 0)
            {
                roleNames.ToList().ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));
            }

            if (roleClaims != null && roleClaims.Count > 0)
            {
                roleClaims.ForEach(x => claims.Add(new Claim(x.Type, x.Value)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

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
            var contentMail = "<a href='" + url + token + "'>Click here reset password: </a>";
            _mailService.SentMail(contentMail, request.Email);

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

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}