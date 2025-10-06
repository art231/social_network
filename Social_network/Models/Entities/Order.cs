namespace Social_network.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        
        public User? User { get; set; }
        public Address? Address { get; set; }
        public Payment? Payment { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
