using BusinessLayer.Abstract;
using BusinessLayer.Utilities.NinjectInstance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialUser.Utilities
{
    public static class PostUtility
    {
        private static IPostService _postService = NinjectInstanceFactory.GetInstance<IPostService>();
        /// <summary>
        /// Save like count
        /// </summary>
        /// <param name="postid">PostId</param>
        /// <param name="currentLike">Post like count</param>
        /// <returns></returns>
        public static async Task UpdateLikeCount(int postid, int currentLike)
        {
            var count = await _postService.FindPost(a => a.PostId == postid);
            count.LikeCount = currentLike;
            await _postService.PostUpdate(count);
        }
    }
}