namespace Social_network.Models.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        
        public Product? Product { get; set; }
    }
}
