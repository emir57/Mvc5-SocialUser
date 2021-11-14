using EntityLayer.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class GroupMessage : IEntity
    {
        [Key]
        public int MessageId { get; set; }
        public int GroupId { get; set; }
        public string SenderUserId { get; set; }
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
