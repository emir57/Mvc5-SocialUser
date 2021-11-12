using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            //Business
            Bind<IChatMessageService>().To<ChatMessageManager>().InSingletonScope();
            Bind<ICommentAnswerService>().To<CommentAnswerManager>().InSingletonScope();
            Bind<ICommentService>().To<CommentManager>().InSingletonScope();
            Bind<IGroupMemberService>().To<GroupMemberManager>().InSingletonScope();
            Bind<IGroupMessageService>().To<GroupMessageManager>().InSingletonScope();
            Bind<IGroupService>().To<GroupManager>().InSingletonScope();
            Bind<IPostLikeService>().To<PostLikeManager>().InSingletonScope();
            Bind<IPostService>().To<PostManager>().InSingletonScope();
            Bind<IUserFriendService>().To<UserFriendManager>().InSingletonScope();
            Bind<IUserService>().To<UserManager>().InSingletonScope();
            Bind<IApplicationUserDal>().To<EfApplicationUserDal>().InSingletonScope();

            //DataAccess
            Bind<IChatMessageDal>().To<EfChatMessageDal>().InSingletonScope();
            Bind<ICommentAnswerDal>().To<EfCommentAnswerDal>().InSingletonScope();
            Bind<ICommentDal>().To<EfCommentDal>().InSingletonScope();
            Bind<IGroupMemberDal>().To<EfGroupMemberDal>().InSingletonScope();
            Bind<IGroupMessageDal>().To<EfGroupMessageDal>().InSingletonScope();
            Bind<IGroupDal>().To<EfGroupDal>().InSingletonScope();
            Bind<IPostLikeDal>().To<EfPostLikeDal>().InSingletonScope();
            Bind<IPostDal>().To<EfPostDal>().InSingletonScope();
            Bind<IUserFriendDal>().To<EfUserFriendDal>().InSingletonScope();

            //Validators
            Bind<IValidator<Post>>().To<PostValidator>().InTransientScope();
            Bind<IValidator<GroupMessage>>().To<GroupMessageValidator>().InTransientScope();
            Bind<IValidator<Comment>>().To<CommentValidator>().InTransientScope();
            Bind<IValidator<CommentAnswer>>().To<CommentAnswerValidator>().InTransientScope();
            Bind<IValidator<ChatMessage>>().To<ChatMessageValidator>().InTransientScope();
        }
    }
}
