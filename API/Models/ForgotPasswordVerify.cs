namespace API.Models
{
    public class ForgotPasswordVerify
    {
        public string? Username { get; set; }
        public string? Pin { get; set; }
        public DateTime RequestDateTime { get; set; } = DateTime.Now;
    }
}
