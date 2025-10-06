namespace Social_network.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "pending";
        public string? ExternalId { get; set; }
        
        public Order? Order { get; set; }
    }
}
