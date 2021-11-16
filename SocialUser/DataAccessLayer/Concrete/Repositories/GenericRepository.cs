using DataAccessLayer.Abstract;
using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T,TContext> : IRepository<T> 
        where T : class, IEntity
        where TContext:DbContext,new()
    {
        TContext _context = new TContext();
        public async Task Delete(T p)
        {
            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> Search(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(filter);
        }

        public async Task Insert(T p)
        {
            _context.Entry(p).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> List()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> List(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task Update(T p)
        {
            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<List<T>> OrderedDateTimeDesc(Expression<Func<T, DateTime>> filter, Expression<Func<T, bool>> search = null)
        {
            return search == null ?
                await _context.Set<T>().OrderByDescending(filter).ToListAsync() :
                await _context.Set<T>().OrderByDescending(filter).Where(search).ToListAsync();
        }
        public async Task<List<T>> OrderedDateTimeAsc(Expression<Func<T, DateTime>> filter, Expression<Func<T, bool>> search = null)
        {
            return search == null ?
                await _context.Set<T>().OrderBy(filter).ToListAsync() :
                await _context.Set<T>().OrderBy(filter).Where(search).ToListAsync();
        }
        public async Task<List<T>> OrderedInt(Expression<Func<T, int>> filter)
        {
            return await _context.Set<T>().OrderByDescending(filter).ToListAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).CountAsync();
        }
        public async Task<List<T>> OrderedTake(Expression<Func<T, int>> filter, int takeCount)
        {
            return await _context.Set<T>().OrderByDescending(filter).Take(takeCount).ToListAsync();
        }
    }
}
