using EntityLayer.Concrete;
using System.Collections.Generic;

namespace SocialUser.Models
{
    public class GetChatViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public ApplicationUser currentUser { get; set; }
        public List<UserFriend> friends { get; set; }
        public List<ChatMessage> msg { get; set; }
        public List<GroupMessage> groupMessage { get; set; }
        public List<Group> groups { get; set; }
        public List<GroupMember> members { get; set; }
        public string userid1 { get; set; }
        public string userid2 { get; set; }
    }
}