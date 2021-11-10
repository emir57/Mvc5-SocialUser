using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentAnswerService
    {
        Task<List<CommentAnswer>> GetAllBL(Expression<Func<CommentAnswer, bool>> filter = null);
        Task CommentAnswerAddBL(CommentAnswer c);
        Task CommentAnswerDeleteBL(CommentAnswer c);
        Task<CommentAnswer> FindPostBL(Expression<Func<CommentAnswer, bool>> filter);
    }
}
