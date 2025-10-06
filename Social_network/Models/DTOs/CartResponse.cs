namespace Social_network.Models.DTOs
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? SessionToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public decimal Total { get; set; }
    }

    public class CartItemResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceSnapshot { get; set; }
        public decimal Total { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class AddCartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}
