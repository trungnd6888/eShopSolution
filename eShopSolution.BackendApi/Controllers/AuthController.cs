using eShopSolution.Application.System.Auth;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Register(request);

            if (!result)
            {
                return Unauthorized(new { error = "Username available" });
            }

            AppUser userRegister = await _authService.GetByUserName(request.UserName);

            return Ok(new { user = userRegister });
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string url = "http://127.0.0.1:5173/ForgotPassword/Reset?Email=" + request.Email + "&Token=";
            bool result = await _authService.ForgotPassword(request, url);
            if (!result) return BadRequest(new { error = "Email không hợp lệ" });

            return Ok("Forgot password success");
        }

        [HttpPost("forgotPassword/reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, [FromQuery] string token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.ResetPassword(request, token);

            if (result["result"] == null) return BadRequest(new { error = result["error"] });

            return Ok();
        }
    }
}