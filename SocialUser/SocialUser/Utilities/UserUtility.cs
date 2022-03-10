using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialUser.Utilities
{
    public static class UserUtility
    {
        private static IUserService _userService = new UserManager(new EfApplicationUserDal());
        /// <summary>
        /// Get Current User
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        public static async Task<ApplicationUser> GetCurrentUser(string id)
        {
            return await _userService.Find(a => a.Id == id);
        }
    }
}