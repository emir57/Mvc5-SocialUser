using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfGroupMemberDal : GenericRepository<GroupMember, SocialUserContext>, IGroupMemberDal
    {
    }
}
