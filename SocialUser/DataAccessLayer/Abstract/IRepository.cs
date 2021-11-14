using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        Task<List<T>> List();
        Task Insert(T p);
        Task Delete(T p);
        Task Update(T p);
        Task<int> Count(Expression<Func<T, bool>> filter);
        Task<List<T>> List(Expression<Func<T, bool>> filter);
        Task<T> Search(Expression<Func<T, bool>> filter);
        Task<List<T>> OrderedDateTimeDesc(Expression<Func<T, DateTime>> filter, Expression<Func<T, bool>> search = null);
        Task<List<T>> OrderedDateTimeAsc(Expression<Func<T, DateTime>> filter, Expression<Func<T, bool>> search = null);
        Task<List<T>> OrderedInt(Expression<Func<T, int>> filter);
        Task<List<T>> OrderedTake(Expression<Func<T, int>> filter, int takeCount);


    }
}
