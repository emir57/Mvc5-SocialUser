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
    public class GroupManager : IGroupService
    {
        IGroupDal _group;
        public GroupManager(IGroupDal group)
        {
            _group = group;
        }

        public async Task Add(Group g)
        {
            await _group.Insert(g);
        }

        public async Task Delete(Group p)
        {
            await _group.Delete(p);
        }

        public async Task<Group> FindGroup(Expression<Func<Group, bool>> filter)
        {
            return await _group.Search(filter);
        }

        public async Task<List<Group>> List(Expression<Func<Group, bool>> filter = null)
        {
            return filter == null ?
                await _group.List() :
                await _group.List(filter);
        }
    }
}
