using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EntityLayer.Concrete
{
    public class CommentAnswer : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string AnswerDescription { get; set; }
        public DateTime AnswerDateTime { get; set; }
    }
}