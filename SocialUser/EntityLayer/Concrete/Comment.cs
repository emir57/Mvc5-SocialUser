using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Comment : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string CommentDescription { get; set; }
        public DateTime CommentDateTime { get; set; }

        
    }
}