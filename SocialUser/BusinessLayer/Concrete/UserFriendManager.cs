using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserFriendManager : IUserFriendService
    {
        private IUserFriendDal _friends;
        public UserFriendManager(IUserFriendDal friends)
        {
            _friends = friends;
        }

        public async Task Add(UserFriend userFriend)
        {
            await _friends.Insert(userFriend);
        }

        public async Task Delete(UserFriend userFriend)
        {
            await _friends.Delete(userFriend);
        }

        public async Task<UserFriend> Find(Expression<Func<UserFriend, bool>> filter)
        {
            return await _friends.Search(filter);
        }

        public async Task<int> FriendCount(Expression<Func<UserFriend, bool>> filter)
        {
            return await _friends.Count(filter);
        }

        public async Task<List<UserFriend>> GetAll(Expression<Func<UserFriend, bool>> filter = null)
        {
            return filter == null ?
                await _friends.List() :
                await _friends.List(filter);
        }

        public async Task Update(UserFriend userFriend)
        {
            await _friends.Update(userFriend);
        }
    }
}
