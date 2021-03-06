using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataAccessLayer.Concrete
{
    public class SocialUserContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAnswer> CommentAnswers { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<GroupMessage> GroupsMessages { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}