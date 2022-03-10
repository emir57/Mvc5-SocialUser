using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class UserFriend: IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public bool Check { get; set; }
    }
}
