using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class PostViewModel
    {
        public List<Post> posts { get; set; }
        public List<PostLike> postLike { get; set; }
        public List<ApplicationUser> users { get; set; }
        
    }
}