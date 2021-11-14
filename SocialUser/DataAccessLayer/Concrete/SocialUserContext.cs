using EntityLayer.Concrete;
using System.Data.Entity;

namespace DataAccessLayer.Concrete
{
    public class SocialUserContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAnswer> CommentAnswers { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<ChatMessage> chatMessages { get; set; }
        public DbSet<UserFriend> userFirends { get; set; }
        public DbSet<GroupMessage> groupsMessages { get; set; }
        public DbSet<GroupMember> groupMembers { get; set; }
        public DbSet<Group> groups { get; set; }
    }
}