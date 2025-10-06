namespace Social_network.Models.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? SessionToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public User? User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
