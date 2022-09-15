#nullable disable
namespace API.Models.Chat
{
    public class GroupMember
    {
        public int Id { get; set; }
        public AppUser User { get; set; }
        public int ConversationId { get; set; }
        public string JoinedDate { get; set; } = DateTime.Now.ToString(Constants.DateTimeFormat);
        public string LeftDate  { get; set; }
    }
}
