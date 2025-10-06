using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserByIdAsync(userId);
            
            if (user == null)
                return NotFound("Пользователь не найден");

            return Ok(user);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] RegisterRequest request)
        {
            var userId = GetCurrentUserId();
            var user = await _userService.UpdateUserAsync(userId, request);
            
            if (user == null)
                return BadRequest("Не удалось обновить данные пользователя");

            return Ok(user);
        }

        [HttpGet("me/orders")]
        public IActionResult GetUserOrders()
        {
            var userId = GetCurrentUserId();
            // Здесь будет вызов OrderService для получения заказов пользователя
            return Ok(new { message = "Метод получения заказов пользователя" });
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
