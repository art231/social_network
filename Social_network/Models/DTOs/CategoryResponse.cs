namespace Social_network.Models.DTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public List<CategoryResponse> Children { get; set; } = new List<CategoryResponse>();
    }
}
