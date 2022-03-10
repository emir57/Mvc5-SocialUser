using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class DetailViewModel
    {
        public string UserProfilePhoto { get; set; }
        public List<Comment> Comments { get; set; }
        public List<CommentAnswer> CommentAnwers { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<PostLike> Likes { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}