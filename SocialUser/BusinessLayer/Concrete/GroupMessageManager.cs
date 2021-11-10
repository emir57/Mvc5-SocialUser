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
    public class GroupMessageManager : IGroupMessageService
    {

        IGroupMessageDal _message;
        public GroupMessageManager(IGroupMessageDal message)
        {
            _message = message;
        }

        public async Task Add(GroupMessage groupMessage)
        {
            await _message.Insert(groupMessage);
        }

        public async Task Delete(GroupMessage groupMessage)
        {
            await _message.Delete(groupMessage);
        }

        public async Task<List<GroupMessage>> GetMessages(Expression<Func<GroupMessage, bool>> filter = null)
        {
            return filter == null ?
                await _message.List() :
                await _message.List(filter);
        }
    }
}
