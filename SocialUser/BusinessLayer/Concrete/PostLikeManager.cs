using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PostLikeManager : IPostLikeService
    {
        private IPostLikeDal _postLike;
        public PostLikeManager(IPostLikeDal postLikeDal)
        {
            _postLike = postLikeDal;
        }

        public async Task PostLikeAdd(PostLike p)
        {
            await _postLike.Insert(p);
        }

        public async Task PostLikeDelete(PostLike p)
        {
            await _postLike.Delete(p);
        }

        public async Task<List<PostLike>> PostLikeList(Expression<Func<PostLike, bool>> filter = null)
        {
            return filter == null ?
                await _postLike.List() : //null
                await _postLike.List(filter); //not null
        }

        public async Task<PostLike> PostLikeFind(Expression<Func<PostLike, bool>> filter)
        {
            return await _postLike .Search(filter);
        }

        public async Task<int> PostLikeCount(Expression<Func<PostLike, bool>> filter)
        {
            return await _postLike.Count(filter);
        }
    }
}
