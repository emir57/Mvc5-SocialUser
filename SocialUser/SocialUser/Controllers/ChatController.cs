using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNet.Identity;
using SocialUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialUser.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        ChatMessageManager _chatMessage = new ChatMessageManager(new EfChatMessageDal());
        UserManager _users = new UserManager(new EfApplicationUserDal());
        UserFriendManager _userFriends = new UserFriendManager(new EfUserFriendDal());
        GroupMessageManager _groupMessages = new GroupMessageManager(new EfGroupMessageDal());
        GroupMemberManager _groupMembers = new GroupMemberManager(new EfGroupMemberDal());
        GroupManager _groups = new GroupManager(new EfGroupDal());
        public async Task<ApplicationUser> getCurrentUser(string id)
        {
            return await _users.Find(a => a.Id == id);
        }
        // GET: Chat
        public async Task<ActionResult> Index(string message)
        {
            string currentUserId = User.Identity.GetUserId();
            GetChatViewModel model = new GetChatViewModel()
            {
                Friends = await _userFriends.GetAll(a => (a.UserId1 == currentUserId || a.UserId2 == currentUserId) && (a.Check == true)),
                Users = await _users.GetAll(),
                Groups = await _groups.List(a => a.CreateGroupUserId == currentUserId)
            };
            ViewBag.message = message;
            return View(model);
        }
        //user1 current user
        public async Task<ActionResult> GetChatView(string user1, string user2)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser getUser1 = await _users.Find(a => a.Id == user1);
            ApplicationUser getUser2 = await _users.Find(a => a.Id == user2);
            GetChatViewModel chat = new GetChatViewModel()
            {
                User1 = getUser1,
                User2 = getUser2,
                CurrentUser = await getCurrentUser(currentUserId)
            };
            return View(chat);
        }
        public async Task<PartialViewResult> LoadChatMessage(string user1, string user2)
        {

            GetChatViewModel chat = new GetChatViewModel()
            {
                ChatMessages = await _chatMessage.GetMessages(user1, user2)
            };
            return PartialView("LoadChatMessage", chat);
        }
        [HttpPost]
        public async Task<ActionResult> ChatDo(string user1, string user2,string text,ChatMessage message)
        {
            message.SenderMessageUser = user1;
            message.RecipientMessageUser = user2;
            message.MessageText = text;
            message.MessageDateTime = DateTime.Now;
            await _chatMessage.AddMessages(message);
            SampleHub.BroadcastChat(user1, user2);
            return RedirectToAction("Index");
        }

        public async Task<PartialViewResult> GetLeftBarFriends()
        {
            string currentUserId = User.Identity.GetUserId();
            GetChatViewModel chat = new GetChatViewModel()
            {
                CurrentUser = await getCurrentUser(currentUserId),
                Users = await _users.GetAll(),
                Friends = await _userFriends.GetAll(a => a.UserId1 == currentUserId || a.UserId2 == currentUserId),
                Groups = await _groups.List(),
                Members = await _groupMembers.List()
            };
            return PartialView("GetFriends",chat);
        }


        //Groups
        public async Task<ActionResult> GetGroupView(int groupId)
        {
            string currentUser = User.Identity.GetUserId();
            GetGroupViewModel chat = new GetGroupViewModel()
            {
                Users = await _users.GetAll(),
                CurrentUser = await getCurrentUser(currentUser),
                GroupMembers = await _groupMembers.List(a => a.GroupId == groupId),
                GroupMessages = await _groupMessages.GetMessages(a => a.GroupId == groupId),
                Group = await _groups.FindGroup(a => a.GroupId == groupId)
            };
            return View(chat);
        }
        public async Task<PartialViewResult> LoadGroupMessage(int? groupId)
        {
            //user1 sender
            GetGroupViewModel chat = new GetGroupViewModel();
            chat.groupMessage = await _groupMessages.GetMessages(a => a.GroupId == groupId);
            chat.users = await _users.GetAll();
            chat.groupId = (int)groupId;
            return PartialView("LoadGroupMessage", chat);
        }

        public async Task<ActionResult> GroupMessageDo(int groupId,string text,GroupMessage message)
        {
            string currentUserId = User.Identity.GetUserId();
            message.GroupId = groupId;
            message.SenderUserId = currentUserId;
            message.Message = text;
            message.MessageDateTime = DateTime.Now;
            await _groupMessages.Add(message);
            SampleHub.BroadcastGroupChat();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CreateGroup(string groupName, Group group,GroupMember m1,GroupMember m2, GroupMember m3)
        {
            var now = DateTime.Now;
            string currentuserid = User.Identity.GetUserId();
            //var currentUser = await getCurrentUser(currentuserid);
            group.GroupName = groupName;
            group.GroupDateTime = now;
            group.CreateGroupUserId = currentuserid;
            
            await _groups.Add(group);

            SampleHub.BroadcastAddFriend();
            return RedirectToAction("Index",new {@message= "Grup başarıyla oluşturuldu.\nGrubu aktif etmek için arkadaşlarınızı ekleyin." });
        }
        public async Task<ActionResult> DeleteGroup(int id)
        {
            var findGroup = await _groups.FindGroup(a => a.GroupId == id);
            if(findGroup!=null)
            {
                await _groups.Delete(findGroup);
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> AddUserGroup(string userId,int groupId,GroupMember m,GroupMember CreateGroupUser)
        {
            if(userId!=null && groupId != 0)
            {
                string currentUserId = User.Identity.GetUserId();
                var getGroup = await _groups.FindGroup(a => a.GroupId == groupId);
                var check = await _groupMembers.FindMember(a => a.GroupId == groupId && a.UserId == getGroup.CreateGroupUserId);
                if(check == null)
                {
                    CreateGroupUser.GroupId = groupId;
                    CreateGroupUser.UserId = getGroup.CreateGroupUserId;
                    CreateGroupUser.Role = "Manager";
                    await _groupMembers.Add(CreateGroupUser);
                }
                //add group members
                m.UserId = userId;
                m.GroupId = groupId;
                m.Role = "User";
                await _groupMembers.Add(m);
                SampleHub.BroadcastAddFriend();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DoManager(int id)
        {
            var getMember = await _groupMembers.FindMember(a => a.MemberId == id);
            var getGroup = await _groups.FindGroup(a => a.GroupId == getMember.GroupId);
            if (getGroup.CreateGroupUserId != getMember.UserId)
            {
                getMember.Role = "Manager";
                await _groupMembers.Update(getMember);
            }
            

            return RedirectToAction("GetGroupView",new { @groupId=getGroup.GroupId });
        }
        public async Task<ActionResult> RemoveManager(int id)
        {
            var getMember = await _groupMembers.FindMember(a => a.MemberId == id);
            var getGroup = await _groups.FindGroup(a => a.GroupId == getMember.GroupId);
            if(getGroup.CreateGroupUserId != getMember.UserId)
            {
                getMember.Role = "User";
                await _groupMembers.Update(getMember);
            }
            

            return RedirectToAction("GetGroupView",new { @groupId = getGroup.GroupId });
        }

        public async Task<ActionResult> GroupRemoveUser(int memberId,int groupId)
        {
            var getMember = await _groupMembers.FindMember(a => a.MemberId == memberId);
            var getGroup = await _groups.FindGroup(a => a.GroupId == groupId);
            if(getGroup.CreateGroupUserId!= getMember.UserId)
            {
                await _groupMembers.Delete(getMember);
            }
            
            return RedirectToAction("GetGroupView", new { @groupId = groupId });
        }

    }
}