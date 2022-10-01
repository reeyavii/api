using Microsoft.AspNetCore.SignalR;

namespace API.Models
{
    public class Verify
    {
        public string UserId { get; set; }
        public string Pin { get; set; }
        public DateTime RequestDateTime { get; set; } = DateTime.Now;
    }
}
