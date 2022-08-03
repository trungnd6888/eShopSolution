using eShopSolution.Application.System.User;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Authenticate(request);
            AppUser userLogin = await _userService.GetByUserName(request.Username);

            if (result["token"] == null || userLogin == null)
            {
                return Unauthorized(new { error = result["error"] });
            }

            return Ok(new
            {
                token = result["token"],
                user = userLogin
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Register(request);

            if (!result)
            {
                return Unauthorized(new { error = "Username available" });
            }

            AppUser userRegister = await _userService.GetByUserName(request.UserName);

            return Ok(new { user = userRegister });
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string url = "http://127.0.0.1:5173/ResetPassword?Email=" + request.Email + "&Token=";
            bool result = await _userService.ForgotPassword(request, url);
            if (!result) return BadRequest(new { error = "Email không hợp lệ" });

            return Ok("Forgot password success");
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, [FromQuery] string token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.ResetPassword(request, token);

            if (result["result"] == null) return BadRequest(new { error = result["error"] });

            return Ok();
        }
    }
}