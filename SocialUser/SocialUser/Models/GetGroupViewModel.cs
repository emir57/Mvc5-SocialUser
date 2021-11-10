using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class GetGroupViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public ApplicationUser userid1 { get; set; }
        public ApplicationUser userid2 { get; set; }
        public ApplicationUser userid3 { get; set; }
        public ApplicationUser currentUser { get; set; }
        public List<GroupMember> groupsMembers { get; set; }
        public List<GroupMessage> groupMessage { get; set; }
        public Group group { get; set; }
        public int groupId { get; set; }
    }
}