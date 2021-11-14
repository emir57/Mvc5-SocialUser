using EntityLayer.Concrete;
using System.Collections.Generic;

namespace SocialUser.Models
{
    public class LastCommentViewModel
    {
        public IEnumerable<Comment> comments { get; set; }

    }
}