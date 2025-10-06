namespace Social_network.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        
        public User? User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
