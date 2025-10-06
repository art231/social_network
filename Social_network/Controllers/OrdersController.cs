using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Models.DTOs;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.CreateOrderAsync(userId, request.PaymentMethod, request.AddressId);
            
            if (order == null)
                return BadRequest("Не удалось создать заказ");

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.GetOrderAsync(userId, id);
            
            if (order == null)
                return NotFound("Заказ не найден");

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _orderService.CancelOrderAsync(userId, id);
            
            if (!result)
                return BadRequest("Не удалось отменить заказ");

            return Ok(new { message = "Заказ отменен" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }

    public class CreateOrderRequest
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public int AddressId { get; set; }
    }
}
