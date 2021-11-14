﻿using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    public class DetailViewModel
    {
        public List<Comment> comment { get; set; }
        public List<CommentAnswer> commentAnswer { get; set; }
        public List<ApplicationUser> user { get; set; }
        public List<PostLike> likes { get; set; }
        public int postid { get; set; }
    }
}