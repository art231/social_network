namespace Social_network.Models.DTOs
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public AddressResponse Address { get; set; } = new AddressResponse();
        public PaymentResponse Payment { get; set; } = new PaymentResponse();
        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    }

    public class OrderItemResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class CreateOrderRequest
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public int AddressId { get; set; }
    }

    public class AddressResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class PaymentResponse
    {
        public int Id { get; set; }
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
    }
}
