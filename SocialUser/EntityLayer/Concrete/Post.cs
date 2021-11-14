﻿using EntityLayer.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Post : IEntity
    {
        [Key]
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserProfilePhoto { get; set; }
        public string PostPicture { get; set; }
        public string Description { get; set; }
        public int LikeCount { get; set; }
        public DateTime PostDateTime { get; set; }
    }
}