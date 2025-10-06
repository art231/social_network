using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CartItemRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.AddItemAsync(userId, request.ProductId, request.Quantity);
            if (!result)
                return BadRequest("Не удалось добавить товар в корзину");

            return Ok(new { message = "Товар добавлен в корзину" });
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] CartItemRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.UpdateItemAsync(userId, id, request.Quantity);
            if (!result)
                return BadRequest("Не удалось обновить товар в корзине");

            return Ok(new { message = "Количество товара обновлено" });
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.RemoveItemAsync(userId, id);
            if (!result)
                return BadRequest("Не удалось удалить товар из корзины");

            return Ok(new { message = "Товар удален из корзины" });
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.ClearCartAsync(userId);
            if (!result)
                return BadRequest("Не удалось очистить корзину");

            return Ok(new { message = "Корзина очищена" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }

    public class CartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
