using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGroupService
    {
        Task<List<Group>> List(Expression<Func<Group, bool>> filter = null);
        Task<Group> FindGroup(Expression<Func<Group, bool>> filter);
        Task Add(Group g);
        Task Delete(Group p);
    }
}
