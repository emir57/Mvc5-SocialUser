using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class GroupMember :IEntity
    {
        [Key]
        public int MemberId { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
