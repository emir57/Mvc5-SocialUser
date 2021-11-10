using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class GroupMemberManager : IGroupMemberService
    {
        IGroupMemberDal _member;
        public GroupMemberManager(IGroupMemberDal member)
        {
            _member = member;
        }
        public async Task Add(GroupMember g)
        {
            await _member.Insert(g);
        }

        public async Task Delete(GroupMember g)
        {
            await _member.Delete(g);
        }

        public async Task<GroupMember> FindMember(Expression<Func<GroupMember, bool>> filter)
        {
            return await _member.Search(filter);
        }

        public async Task<List<GroupMember>> List(Expression<Func<GroupMember, bool>> filter = null)
        {
            return filter == null ?
                await _member.List() :
                await _member.List(filter);
        }

        public async Task Update(GroupMember g)
        {
            await _member.Update(g);
        }
    }
}
