using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ChatMessage : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string SenderMessageUser { get; set; }
        public string RecipientMessageUser { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDateTime {get;set;}
    }
}
