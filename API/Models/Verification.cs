namespace API.Models
{
    public class Verification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Pin { get; set; }
        public DateTime IssuedDateTime { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}
