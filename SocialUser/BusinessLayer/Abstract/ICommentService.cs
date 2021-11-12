using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAll(Expression<Func<Comment, bool>> filter = null);
        Task CommentAdd(Comment c);
        Task CommentDelete(Comment c);
        Task<int> CommentCount(Expression<Func<Comment, bool>> filter);
        Task<Comment> FindComment(Expression<Func<Comment, bool>> filter);
        Task<List<Comment>> GetCommentListOrderedDateTime(Expression<Func<Comment, DateTime>> filter, Expression<Func<Comment, bool>> search = null);
        Task<List<Comment>> GetCommentListOrderedInt(Expression<Func<Comment, int>> filter);
        Task<List<Comment>> GetCommentListOrderedIdTake(Expression<Func<Comment, int>> filter, int takeCount);

    }
}
