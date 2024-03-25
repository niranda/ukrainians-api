using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ukrainians.UtilityServices.Models.Auth;
using Ukrainians.UtilityServices.Services.Auth;

namespace NomadChat.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        // POST: Auth/Login
        public async Task<IActionResult> Login(AuthModel authModel)
        {
            var result = await _authService.Login(authModel);
            if (result.IsSuccess)
                HttpContext.Response.Cookies.Append("Token", result.Token, new CookieOptions() { SameSite = SameSiteMode.None, Secure = true });

            return Ok(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        // POST: Auth/Signup
        public async Task<IActionResult> Signup(AuthModel authModel)
        {
            var result = await _authService.Signup(authModel);
            if (result.IsSuccess)
                HttpContext.Response.Cookies.Append("Token", result.Token, new CookieOptions() { SameSite = SameSiteMode.None, Secure = true });

            return Ok(result.ErrorMessage);
        }

        [Authorize]
        [HttpPost("Logout")]
        // POST: Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Token");

            return Ok(true);
        }

        [HttpGet("EmailConfirmation")]
        // GET
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmail(email, token);

            return Ok(result);
        }
    }
}
