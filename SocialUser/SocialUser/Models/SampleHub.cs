using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialUser.Models
{
    [HubName("sampleHub")]
    public class SampleHub : Hub
    {
        //refresh posts
        public static void BroadcastPost()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshPostData();
        }
        //refresh comments
        public static void BroadcastComment()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshCommentData();
        }
        //refresh group chat
        public static void BroadcastGroupChat()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshGroupChat();
        }
        //refresh group add and friend add in Chat left bar
        public static void BroadcastAddFriend()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshAddFriend();
        }
        //refresh friend add in profile
        public static void BroadcastFriend(string User1,string User2)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshFriendData(User1,User2);
        }
        //refresh chat
        public static void BroadcastChat(string currentUserId,string userId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SampleHub>();
            context.Clients.All.refreshChatData(currentUserId,userId);
        }
    }
}