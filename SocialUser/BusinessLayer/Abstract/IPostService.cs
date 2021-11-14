using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPostService
    {
        Task<List<Post>> GetAll(Expression<Func<Post, bool>> filter = null);
        Task PostAdd(Post p);
        Task PostDelete(Post p);
        Task PostUpdate(Post p);
        Task<int> PostCount(Expression<Func<Post, bool>> filter);
        Task<Post> FindPost(Expression<Func<Post, bool>> filter);
        Task<List<Post>> GetPostListOrdered(Expression<Func<Post, DateTime>> filter, Expression<Func<Post, bool>> search = null);
    }
}
