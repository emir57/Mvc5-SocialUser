using EntityLayer.Concrete;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
