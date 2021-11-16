using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfUserFriendDal : GenericRepository<UserFriend, SocialUserContext>, IUserFriendDal
    {
    }
}
