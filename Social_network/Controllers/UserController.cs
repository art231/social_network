using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var id = await _userService.RegisterAsync(request);
            return Ok(new { id });
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? firstName, [FromQuery] string? lastName)
        {
            var users = await _userService.SearchAsync(firstName, lastName);
            return Ok(users);
        }
    }
}
