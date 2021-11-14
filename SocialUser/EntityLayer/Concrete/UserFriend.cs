using EntityLayer.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class UserFriend : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public bool Check { get; set; }
    }
}
