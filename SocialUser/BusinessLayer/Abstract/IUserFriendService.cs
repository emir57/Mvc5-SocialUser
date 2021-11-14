using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserFriendService
    {
        Task Add(UserFriend userFriend);

        Task Delete(UserFriend userFriend);
        Task Update(UserFriend userFriend);

        Task<List<UserFriend>> GetAll(Expression<Func<UserFriend, bool>> filter = null);
        Task<int> FriendCount(Expression<Func<UserFriend, bool>> filter);

        Task<UserFriend> Find(Expression<Func<UserFriend, bool>> filter);
    }
}
