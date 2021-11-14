using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAll(Expression<Func<ApplicationUser, bool>> filter = null);
        Task<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> filter);

        Task UpdateUser(ApplicationUser user);
    }
}
