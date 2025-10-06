using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);
            if (result == null)
                return BadRequest("Пользователь с таким email уже существует");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginAsync(request);
            if (result == null)
                return Unauthorized("Неверные учетные данные");

            return Ok(result);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            // TODO: Implement refresh token logic
            return Ok(new { message = "Refresh endpoint - to be implemented" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // TODO: Implement logout logic (blacklist token)
            return Ok(new { message = "Logged out successfully" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Неверный идентификатор пользователя");
            }
            return userId;
        }
    }
}
