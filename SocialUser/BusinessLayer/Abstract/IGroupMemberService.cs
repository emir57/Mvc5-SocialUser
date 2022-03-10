using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGroupMemberService
    {
        Task<List<GroupMember>> List(Expression<Func<GroupMember,bool>>filter=null);
        Task<GroupMember> FindMember(Expression<Func<GroupMember, bool>> filter);
        Task Add(GroupMember g);
        Task Update(GroupMember g);
        Task Delete(GroupMember g);
    }
}
