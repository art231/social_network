using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _userService.LoginAsync(request);
            if (token == null)
                return Unauthorized("Неверные учетные данные");

            return Ok(new { token });
        }
    }
}
