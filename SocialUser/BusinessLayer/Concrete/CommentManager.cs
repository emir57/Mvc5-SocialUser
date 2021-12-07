using BusinessLayer.Abstract;
using BusinessLayer.Utilities.ValidationTool;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        private ICommentDal _comment;
        private IValidator<Comment> _commentValidator;

        public CommentManager(ICommentDal commentDal, IValidator<Comment> commentValidator)
        {
            _comment = commentDal;
            _commentValidator = commentValidator;
        }

        public async Task CommentAdd(Comment c)
        {
            if (ValidationTool.Validate(_commentValidator,c))
            {
                await _comment.Insert(c);
            }

        }

        public async Task<int> CommentCount(Expression<Func<Comment, bool>> filter)
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

        public async Task<List<Comment>> GetCommentListOrderedDateTime(Expression<Func<Comment, DateTime>> filter, Expression<Func<Comment, bool>> search = null)
        {
            return search == null ?
                await _comment.OrderedDateTimeDesc(filter) :
                await _comment.OrderedDateTimeDesc(filter, search);
        }

        public async Task<List<Comment>> GetCommentListOrderedIdTake(Expression<Func<Comment, int>> filter, int takeCount)
        {
            return await _comment.OrderedTake(filter, takeCount);
        }

        public async Task<List<Comment>> GetCommentListOrderedInt(Expression<Func<Comment, int>> filter)
        {
            return await _comment.OrderedInt(filter);
        }
    }
}
