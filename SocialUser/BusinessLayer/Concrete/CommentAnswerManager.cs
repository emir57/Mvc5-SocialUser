using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentAnswerManager : ICommentAnswerService
    {
        private ICommentAnswerDal _answer;
        private IValidator<CommentAnswer> _commentAnswerValidator;

        public CommentAnswerManager(ICommentAnswerDal commentAnswerDal, IValidator<CommentAnswer> commentAnswerValidator)
        {
            _answer = commentAnswerDal;
            _commentAnswerValidator = commentAnswerValidator;
        }
        public async Task Add(CommentAnswer c)
        {
            if (!(_commentAnswerValidator.Validate(c).Errors.Count > 0))
            {
                await _answer.Insert(c);
            }

        }

        public async Task Delete(CommentAnswer c)
        {
            await _answer.Delete(c);
        }

        public async Task<CommentAnswer> FindPost(Expression<Func<CommentAnswer, bool>> filter)
        {
            return await _answer.Search(filter);
        }

        public async Task<List<CommentAnswer>> GetAll(Expression<Func<CommentAnswer, bool>> filter = null)
        {
            return filter == null ?
                await _answer.List() : //null
                await _answer.List(filter); //not null
        }
    }
}
