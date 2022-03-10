using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class ProfileView
    {
        public ApplicationUser User { get; set; }
        public List<Post> Posts { get; set; }
        public int PostCount { get; set; }
        public int FriendsCount { get; set; }
        public bool IsFriend { get; set; }
        public int UserFriendId { get; set; }

    }
}