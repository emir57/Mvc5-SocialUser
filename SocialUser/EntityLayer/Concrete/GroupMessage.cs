using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class GroupMessage:IEntity
    {
        [Key]
        public int MessageId { get; set; }
        public int GroupId { get; set; }
        public string SenderUserId { get; set; }
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
