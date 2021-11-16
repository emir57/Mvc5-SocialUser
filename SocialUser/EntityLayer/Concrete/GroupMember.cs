using EntityLayer.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class GroupMember : IEntity
    {
        [Key]
        public int MemberId { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }

        public Group Group { get; set; }
    }
}
