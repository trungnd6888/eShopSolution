using eShopSolution.Application.System.Auth;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static eShopSolution.Utilities.Contants.SystemContants;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/api/public/[controller]/authenticate")]
        public async Task<IActionResult> AuthenticatePublic([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Authenticate(request);

            if (result["token"] == null)
            {
                return Unauthorized(new { error = result["error"] });
            }

            return Ok(result["token"]);
        }

        [HttpPost("/api/public/[controller]/register")]
        public async Task<IActionResult> RegisterPublic([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Register(request);

            if (!result)
            {
                return Unauthorized(new { error = "Username available" });
            }

            AppUser userRegister = await _authService.GetByUserName(request.UserName);

            if (userRegister != null)
            {
                userRegister.AppUserRoles = new List<AppUserRole>()
                {
                   new AppUserRole()
                    {
                        UserId = userRegister.Id,
                        RoleId = (int)RoleId.PUBLIC,
                    }
                };
            }

            await _authService.SaveChange();

            return Ok(new { user = userRegister });
        }

        [HttpPost("/api/public/[controller]/forgotPassword")]
        public async Task<IActionResult> ForgotPasswordPublic([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string url = "http://127.0.0.1:5173/ForgotPassword/Reset?Email=" + request.Email + "&Token=";
            bool result = await _authService.ForgotPassword(request, url);
            if (!result) return BadRequest(new { error = "Email không hợp lệ" });

            return Ok("Forgot password success");
        }

        [HttpPost("/api/public/[controller]/forgotPassword/reset")]
        public async Task<IActionResult> ResetPasswordPublic([FromBody] ResetPasswordRequest request, [FromQuery] string token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.ResetPassword(request, token);

            if (result["result"] == null) return BadRequest(new { error = result["error"] });

            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Authenticate(request);

            if (result["token"] == null)
            {
                return Unauthorized(new { error = result["error"] });
            }

            return Ok(result["token"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Register(request);

            if (!result)
            {
                return Unauthorized(new { error = "Username available" });
            }

            AppUser userRegister = await _authService.GetByUserName(request.UserName);

            if (userRegister != null)
            {
                userRegister.AppUserRoles = new List<AppUserRole>()
                {
                   new AppUserRole()
                    {
                        UserId = userRegister.Id,
                        RoleId = (int)RoleId.MANAGER,
                    }
                };
            }

            await _authService.SaveChange();

            return Ok(new { user = userRegister });
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string url = "http://127.0.0.1:5173/ForgotPassword/Reset?Email=" + request.Email + "&Token=";
            bool result = await _authService.ForgotPassword(request, url);
            if (!result) return BadRequest(new { error = "Email không hợp lệ" });

            return Ok("Forgot password success");
        }

        [HttpPost("forgotPassword/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, [FromQuery] string token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.ResetPassword(request, token);

            if (result["result"] == null) return BadRequest(new { error = result["error"] });

            return Ok();
        }
    }
}