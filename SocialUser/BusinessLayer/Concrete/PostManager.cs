using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PostManager : IPostService
    {
        private IPostDal _postDal;
        private IValidator<Post> _postValidator;

        public PostManager(IPostDal postDal, IValidator<Post> postValidator)
        {
            _postDal = postDal;
            _postValidator = postValidator;
        }

        public async Task<List<Post>> GetAll(Expression<Func<Post, bool>> filter = null)
        {

            return filter == null ?
                await _postDal.List() : //null
                await _postDal.List(filter); //not null
        }
        public async Task PostAdd(Post p)
        {
            if (!(_postValidator.Validate(p).Errors.Count > 0))
            {
                await _postDal.Insert(p);
            }

        }
        public async Task PostDelete(Post p)
        {
            await _postDal.Delete(p);
        }
        public async Task<Post> FindPost(Expression<Func<Post, bool>> filter)
        {
            return await _postDal.Search(filter);
        }

        public async Task<List<Post>> GetPostListOrdered(Expression<Func<Post, DateTime>> filter, Expression<Func<Post, bool>> search = null)
        {
            return search == null ?
                await _postDal.OrderedDateTimeDesc(filter) :
                await _postDal.OrderedDateTimeDesc(filter, search);
        }

        public async Task PostUpdate(Post p)
        {
            if (!(_postValidator.Validate(p).Errors.Count > 0))
            {
                await _postDal.Update(p);
            }
        }

        public async Task<int> PostCount(Expression<Func<Post, bool>> filter)
        {
            return await _postDal.Count(filter);
        }
    }
}
