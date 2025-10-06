using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var userId = GetCurrentUserId();
            var payment = await _paymentService.CreatePaymentAsync(userId, request.OrderId, request.Provider);
            
            if (payment == null)
                return BadRequest("Не удалось создать платеж");

            return Ok(payment);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] PaymentWebhookRequestDto request)
        {
            // В реальном приложении здесь должна быть верификация подписи
            var webhookRequest = new PaymentWebhookRequest
            {
                PaymentId = request.PaymentId,
                Status = request.Status,
                ExternalId = request.ExternalId,
                Signature = request.Signature
            };
            
            var result = await _paymentService.ProcessWebhookAsync(webhookRequest);
            
            if (!result)
                return BadRequest("Не удалось обработать вебхук");

            return Ok(new { message = "Вебхук обработан" });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPayment(int id)
        {
            var userId = GetCurrentUserId();
            var payment = await _paymentService.GetPaymentAsync(userId, id);
            
            if (payment == null)
                return NotFound("Платеж не найден");

            return Ok(payment);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }

    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }
        public string Provider { get; set; } = string.Empty;
    }

    public class PaymentWebhookRequestDto
    {
        public string PaymentId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
