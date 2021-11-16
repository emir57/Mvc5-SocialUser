using EntityLayer.Concrete;
using System.Collections.Generic;

namespace SocialUser.Models
{
    public class DetailViewModel
    {
        public Post Post { get; set; }
        public List<Comment> comment { get; set; }
        public List<CommentAnswer> commentAnswer { get; set; }
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> users { get; set; }
        public List<PostLike> likes { get; set; }
        public int postid { get; set; }
        public int LikeCount { get; set; }
        
    }
}