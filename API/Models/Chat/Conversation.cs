
#nullable disable
namespace API.Models.Chat
{
    public class Conversation
    {
        public int Id { get; set; }
        public string ConvoName { get; set; }
        public virtual List<GroupMember> GroupMembers { get; set; }

    }
}
