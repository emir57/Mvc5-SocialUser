using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class GetChatViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public List<UserFriend> Friends { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }
        public List<GroupMessage> GroupMessages { get; set; }
        public List<Group> Groups { get; set; }
        public List<GroupMember> Members { get; set; }
        public string Userid1 { get; set; }
        public string Userid2 { get; set; }
        public string Userid3 { get; set; }
    }
}