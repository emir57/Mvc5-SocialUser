using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPostLikeService
    {
        Task<List<PostLike>> PostLikeList(Expression<Func<PostLike, bool>> filter = null);
        Task<PostLike> PostLikeFind(Expression<Func<PostLike, bool>> filter);
        Task<int> PostLikeCount(Expression<Func<PostLike, bool>> filter);
        Task PostLikeAdd(PostLike p);
        Task PostLikeDelete(PostLike p);
    }
}
