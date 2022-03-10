using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<ApplicationUser> Users { get; set; }
        
    }
}