using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Group : IEntity
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime GroupDateTime { get; set; }
        public string CreateGroupUserId { get; set; }

    }
}
