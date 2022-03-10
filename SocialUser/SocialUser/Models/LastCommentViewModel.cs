using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class LastCommentViewModel
    {
        public IEnumerable<Comment> comments { get; set; }

    }
}