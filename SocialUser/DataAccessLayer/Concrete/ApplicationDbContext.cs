using EntityLayer.Concrete;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Concrete
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SocialUserContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
