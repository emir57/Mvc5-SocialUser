using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class DetailViewModel
    {
        public List<Comment> c { get; set; }
        public List<CommentAnswer> cA { get; set; }
        public List<ApplicationUser> user { get; set; }
        public List<PostLike> likes { get; set; }
        public int postid { get; set; }
    }
}