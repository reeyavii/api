#nullable disable
using API.Models.Chat;

namespace API.Models
{
    public class Message
    {
        
        public string MessageId { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public Conversation Conversation { get; set; }
        public string Sent { get; set; } = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss tt");
    }
}
