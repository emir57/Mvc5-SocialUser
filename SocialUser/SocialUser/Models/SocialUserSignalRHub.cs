using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SocialUser.Models
{
    [HubName("sampleHub")]
    public class SocialUserSignalRHub : Hub
    {
        /// <summary>
        /// refresh posts
        /// </summary>
        public static void BroadcastPost()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshPostData();
        }
        /// <summary>
        /// refresh comments
        /// </summary>
        public static void BroadcastComment()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshCommentData();
        }
        /// <summary>
        /// refresh group chat
        /// </summary>
        public static void BroadcastGroupChat()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshGroupChat();
        }
        /// <summary>
        /// refresh group add and friend add in Chat left bar
        /// </summary>
        public static void BroadcastAddFriend()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshAddFriend();
        }
        /// <summary>
        /// refresh friend add in profile
        /// </summary>
        /// <param name="User1">User1</param>
        /// <param name="User2">User2</param>
        public static void BroadcastFriend(string User1, string User2)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshFriendData(User1, User2);
        }
        /// <summary>
        /// refresh chat
        /// </summary>
        /// <param name="currentUserId">Authenticate User</param>
        /// <param name="userId">Other User</param>
        public static void BroadcastChat(string currentUserId, string userId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SocialUserSignalRHub>();
            context.Clients.All.refreshChatData(currentUserId, userId);
        }
    }
}