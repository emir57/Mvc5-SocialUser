using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IApplicationUserDal
    {
        Task<List<ApplicationUser>> List(Expression<Func<ApplicationUser, bool>> filter);
        Task<ApplicationUser> Search(Expression<Func<ApplicationUser, bool>> filter);
        Task<List<ApplicationUser>> List();
        Task Insert(ApplicationUser p);
        Task Delete(ApplicationUser p);
        Task Update(ApplicationUser p);
    }
}
