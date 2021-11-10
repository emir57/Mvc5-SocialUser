using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfApplicationUserDal : IApplicationUserDal
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public async Task Delete(ApplicationUser p)
        {
            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Insert(ApplicationUser p)
        {
            _context.Entry(p).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> List(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _context.Users.Where(filter).ToListAsync();
        }

        public async Task<List<ApplicationUser>> List()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> Search(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _context.Users.SingleOrDefaultAsync(filter);
        }

        public async Task Update(ApplicationUser p)
        {
            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
