using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        IApplicationUserDal _user;
        public UserManager(IApplicationUserDal user)
        {
            _user = user;
        }
        public async Task<ApplicationUser> Find(Expression<Func<ApplicationUser,bool>>filter)
        {
            //return await _context.Users.SingleOrDefaultAsync(filter);
            return await _user.Search(filter);
        }

        public async Task<List<ApplicationUser>> GetAll(Expression<Func<ApplicationUser, bool>> filter=null)
        {
            return filter == null ?
                await _user.List() :
                await _user.List(filter);
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            await _user.Update(user);
        }
    }
}
