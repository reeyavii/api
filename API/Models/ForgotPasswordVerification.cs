namespace API.Models
{
    public class ForgotPasswordVerification
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Pin { get; set; }
        public DateTime? IssuedDateTime { get; set; }
        public DateTime? ExpirationDateTime { get; set; }

    }
}
