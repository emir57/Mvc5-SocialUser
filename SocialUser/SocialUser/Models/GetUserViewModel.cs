using EntityLayer.Concrete;
using System.Collections.Generic;

namespace SocialUser.Models
{
    public class GetUserViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public List<UserFriend> userFriend { get; set; }
    }
}