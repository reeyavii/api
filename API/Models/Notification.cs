namespace API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? UserId { get; set;}
        public string? Title { get; set; }
        public string? Created { get; set; } = DateTime.Now.ToString(Constants.DateTimeFormat);
        public string? Message { get; set; }
        public string? MessageStatus { get; set; }

    }
}
