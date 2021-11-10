using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGroupMessageService
    {
        Task Add(GroupMessage groupMessage);
        Task Delete(GroupMessage groupMessage);
        Task<List<GroupMessage>> GetMessages(Expression<Func<GroupMessage,bool>> filter=null);
    }
}
