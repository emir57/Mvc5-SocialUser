using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentAnswerService
    {
        Task<List<CommentAnswer>> GetAll(Expression<Func<CommentAnswer, bool>> filter = null);
        Task Add(CommentAnswer c);
        Task Delete(CommentAnswer c);
        Task<CommentAnswer> FindPost(Expression<Func<CommentAnswer, bool>> filter);
    }
}
