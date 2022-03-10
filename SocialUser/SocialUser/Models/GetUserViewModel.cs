using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class GetUserViewModel
    {
        public List<ApplicationUser> users { get; set; }
        public List<UserFriend> userFriend { get; set; }
    }
}