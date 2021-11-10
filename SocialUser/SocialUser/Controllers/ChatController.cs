﻿using BusinessLayer.Concrete;
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
            GetChatViewModel model = new GetChatViewModel();
            string currentUserId = User.Identity.GetUserId();
            model.friends = await _userFriends.GetAll(a => (a.UserId1 == currentUserId || a.UserId2 == currentUserId)&&(a.Check==true));
            model.users = await _users.GetAll();
            model.groups = await _groups.List(a=>a.CreateGroupUserId==currentUserId);

            ViewBag.message = message;
            return View(model);
        }
        //user1 current user
        public async Task<ActionResult> GetChatView(string user1, string user2)
        {
            string currentUserId = User.Identity.GetUserId();
            GetChatViewModel chat = new GetChatViewModel();
            //search
            var userid1 = await _users.Find(a => a.Id == user1);
            var userid2 = await _users.Find(a => a.Id == user2);

            chat.userid1 = user1;
            chat.userid2 = user2;
            chat.currentUser = await getCurrentUser(currentUserId);


            ViewBag.userid1ProfilePhoto = userid1.profilePhoto;
            ViewBag.userid2ProfilePhoto = userid2.profilePhoto;

            ViewBag.userid1userName = userid1.UserName;
            ViewBag.userid2userName = userid2.UserName;

            return View(chat);
        }
        public async Task<PartialViewResult> LoadChatMessage(string user1, string user2)
        {

            GetChatViewModel chat = new GetChatViewModel();
            chat.msg = await _chatMessage.GetMessages(user1, user2);
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
            GetChatViewModel chat = new GetChatViewModel();
            string currentUserId = User.Identity.GetUserId();
            chat.currentUser = await getCurrentUser(currentUserId);
            chat.users = await _users.GetAll();
            chat.friends = await _userFriends.GetAll(a => a.UserId1 == currentUserId || a.UserId2 == currentUserId);
            //get current user group id
            var userGroups = await _groupMembers.List(a => a.UserId == currentUserId);
            //get groups
            chat.groups = await _groups.List();
            //get members
            chat.members = await _groupMembers.List();
            return PartialView("GetFriends",chat);
        }


        //Groups
        public async Task<ActionResult> GetGroupView(int groupId)
        {
            GetGroupViewModel chat = new GetGroupViewModel();
            string currentUser = User.Identity.GetUserId();
            chat.users = await _users.GetAll();
            chat.currentUser = await getCurrentUser(currentUser);
            chat.groupsMembers = await _groupMembers.List(a => a.GroupId == groupId);
            chat.currentUser = await _users.Find(a => a.Id == currentUser);
            chat.groupMessage = await _groupMessages.GetMessages(a=>a.GroupId==groupId);
            chat.group = await _groups.FindGroup(a => a.GroupId == groupId);
            chat.groupId = groupId;
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