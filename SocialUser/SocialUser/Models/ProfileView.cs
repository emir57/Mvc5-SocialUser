using EntityLayer.Concrete;
using System.Collections.Generic;

namespace SocialUser.Models
{
    public class ProfileView
    {
        public ApplicationUser user { get; set; }
        public List<Post> posts { get; set; }
        public int postCount { get; set; }
        public int friendsCount { get; set; }
        public bool isFriend { get; set; }
        public int userFriendId { get; set; }

    }
}