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
    public class CommentManager : ICommentService
    {
        ICommentDal _comment;

        public CommentManager(ICommentDal commentDal)
        {
            _comment = commentDal;
        }

        public async Task CommentAdd(Comment c)
        {
            await _comment.Insert(c);
        }

        public async Task<int> CommentCount(Expression<Func<Comment,bool>>filter)
        {
            return await _comment.Count(filter);
        }

        public async Task CommentDelete(Comment c)
        {
            await _comment.Delete(c);
        }

        public async Task<Comment> FindComment(Expression<Func<Comment, bool>> filter)
        {
            return await _comment.Search(filter);
        }

        public async Task<List<Comment>> GetAll(Expression<Func<Comment, bool>> filter = null)
        {
            return filter == null ?
                await _comment.List() : //null
                await _comment.List(filter); //not null
        }

        public async Task<List<Comment>> GetCommentListOrderedDateTime(Expression<Func<Comment, DateTime>> filter,Expression<Func<Comment,bool>>search=null)
        {
            return search == null ?
                await _comment.OrderedDateTimeDesc(filter):
                await _comment.OrderedDateTimeDesc(filter,search);
        }

        public async Task<List<Comment>> GetCommentListOrderedIdTake(Expression<Func<Comment, int>> filter,int takeCount)
        {
            return await _comment.OrderedTake(filter, takeCount);
        }

        public async Task<List<Comment>> GetCommentListOrderedInt(Expression<Func<Comment, int>> filter)
        {
            return await _comment.OrderedInt(filter);
        }
    }
}
