using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class GetGroupViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
        public ApplicationUser User3 { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public List<GroupMember> GroupMembers { get; set; }
        public List<GroupMessage> GroupMessages { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
    }
}