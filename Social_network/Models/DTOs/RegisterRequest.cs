namespace Social_network.Models.DTOs
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
