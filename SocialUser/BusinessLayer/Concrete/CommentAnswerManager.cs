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
    public class CommentAnswerManager : ICommentAnswerService
    {
        ICommentAnswerDal _answer;

        public CommentAnswerManager(ICommentAnswerDal commentAnswerDal)
        {
            _answer = commentAnswerDal;
        }

        GenericRepository<CommentAnswer> commentAnswers = new GenericRepository<CommentAnswer>();

        public async Task CommentAnswerAddBL(CommentAnswer c)
        {
            await _answer.Insert(c);
        }

        public async Task CommentAnswerDeleteBL(CommentAnswer c)
        {
            await _answer.Delete(c);
        }

        public async Task<CommentAnswer> FindPostBL(Expression<Func<CommentAnswer, bool>> filter)
        {
            return await _answer.Search(filter);
        }

        public async Task<List<CommentAnswer>> GetAllBL(Expression<Func<CommentAnswer, bool>> filter = null)
        {
            return filter == null ?
                await _answer.List() : //null
                await _answer .List(filter); //not null
        }
    }
}
