namespace Social_network.Models.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        public int CategoryId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public List<ProductImageResponse> Images { get; set; } = new List<ProductImageResponse>();
        public CategoryResponse? Category { get; set; }
    }

    public class ProductImageResponse
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
