using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNet.Identity;
using SocialUser.Models;
using SocialUser.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialUser.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        PostManager _posts = new PostManager(new EfPostDal());
        CommentManager _comments = new CommentManager(new EfCommentDal());
        CommentAnswerManager _commentAnswers = new CommentAnswerManager(new EfCommentAnswerDal());
        PostLikeManager _postLikes = new PostLikeManager(new EfPostLikeDal());
        UserManager _users = new UserManager(new EfApplicationUserDal());
        UserFriendManager _userFriend = new UserFriendManager(new EfUserFriendDal());

        //get current user
        public async Task<ApplicationUser> getCurrentUser(string id)
        {
            return await _users.Find(a => a.Id == id);
        }
        //save like count
        public async Task updateLikeCount(int postid, int currentLike)
        {
            var count = await _posts.FindPost(a => a.PostId == postid);
            count.LikeCount = currentLike;
            await _posts.PostUpdate(count);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            string currentId = User.Identity.GetUserId();
            ViewBag.id = currentId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostAdd(string description,HttpPostedFileBase picture,Post post)
        {
            string databasePath = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentId = User.Identity.GetUserId();
                var currentUser = await getCurrentUser(currentId);
                if (picture != null)
                {
                    string path = Server.MapPath("~/Content/postPicture/");
                    ImageUtility.UploadImage(picture, out databasePath, path);
                    
                }
                else
                    databasePath = ""; 
                post.Description = description;
                post.LikeCount = 0;
                post.PostPicture = databasePath;
                post.UserProfilePhoto = currentUser.profilePhoto;
                post.Username = currentUser.UserName;
                post.UserId = currentUser.Id;
                post.PostDateTime = DateTime.Now;
                await _posts.PostAdd(post);
                SampleHub.BroadcastPost();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> PostDelete(int id)
        {
            var post = await _posts.FindPost(a => a.PostId == id);
            var comments = await _comments.GetAll(a => a.PostId == id);
            var commentAnswers = await _commentAnswers.GetAllBL();
            if (!(String.IsNullOrEmpty(post.PostPicture)))
            {
                string[] pictureName = post.PostPicture.Split('/');
                string picturePath = Server.MapPath("~/Content/postPicture/");
                string fullPath = picturePath + pictureName[4];
                ImageUtility.DeleteImage(fullPath);
            }
            await _posts.PostDelete(post);
            foreach (var comment in comments)
            {
                foreach (var answers in commentAnswers)
                {
                    if (answers.CommentId == comment.Id)
                        await _commentAnswers.CommentAnswerDeleteBL(answers);
                }
                await _comments.CommentDelete(comment);
            }
            SampleHub.BroadcastPost();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> PostDetail(int? postid)
        {
            Post post = await _posts.FindPost(a => a.PostId == postid);
            if (post == null)
                return RedirectToAction("Index");
            else
            {
                string currentUserId = User.Identity.GetUserId();
                //do current user like post? null=not like -- /user not like = false -- /user like = true
                PostLike search = await _postLikes.PostLikeFind(a => a.UserId == currentUserId && a.PostId == postid);
                if(search == null)
                {
                    ViewData["checkLike"] = false;
                }
                else { ViewData["checkLike"] = true; }
                string postUserId = post.UserId;
                ApplicationUser user = await _users.Find(a => a.Id == postUserId);
                List<Comment> comments = await _comments.GetAll(a => a.PostId == post.PostId);
                DetailViewModel model = new DetailViewModel()
                {
                    UserProfilePhoto = user.profilePhoto,
                    Comments = await _comments.GetCommentListOrderedDateTime(a => a.CommentDateTime),
                    CommentAnwers = await _commentAnswers.GetAllBL(a => a.CommentId == postid),
                    Likes = await _postLikes.PostLikeList(a => a.PostId == postid),
                    Users = await _users.GetAll(),
                    Post = post
                };
                return View(model);
            }
        }
        public async Task<ActionResult> PostLike(int? postid, bool? check)
        {
            if (User.Identity.IsAuthenticated)
            {
                PostLike like = new PostLike();
                string currentUserId = User.Identity.GetUserId();
                if(postid != null && check==false)
                {
                    like.UserId = currentUserId;
                    like.PostId = (int)postid;
                    await _postLikes.PostLikeAdd(like);
                    var likes = await _postLikes.PostLikeList(a => a.PostId == postid);
                    int likecount = likes.Count();
                    await updateLikeCount((int)postid, likecount);
                    SampleHub.BroadcastPost();

                    return RedirectToAction("PostDetail",new { @postid = postid });
                }
                else if(postid!= null && check==true)
                {
                    var search = await _postLikes.PostLikeFind(a => a.PostId == postid && a.UserId == currentUserId);
                    await _postLikes.PostLikeDelete(search);
                    var likes = await _postLikes.PostLikeList(a => a.PostId == postid);
                    int likecount = likes.Count();
                    await updateLikeCount((int)postid, likecount);
                    SampleHub.BroadcastPost();

                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> PostDoComment(int? postid, string text,Comment comment)
        {
            text = text.TrimStart().TrimEnd();
            var post = await _posts.FindPost(a => a.PostId == postid);
            if (post == null)
                return RedirectToAction("Index");
            else
            {
                if(User.Identity.IsAuthenticated)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var user = await getCurrentUser(currentUserId);

                    comment.PostId = post.PostId;
                    comment.UserName = user.UserName;
                    comment.UserId = user.Id;
                    comment.CommentDescription = text;
                    comment.CommentDateTime = DateTime.Now;
                    await _comments.CommentAdd(comment);
                    SampleHub.BroadcastComment();
                    return RedirectToAction("PostDetail", new{ @postid = postid });
                }
                else
                    return RedirectToAction("PostDetail", postid);
            }
        }
        public async Task<ActionResult> PostCommentDelete(int? id,int postid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var comment = await _comments.FindComment(a => a.Id == id);
                var commentAnswers = await _commentAnswers.GetAllBL(a=>a.CommentId==comment.Id);
                if (comment == null){ return RedirectToAction("Index"); }
                else
                {
                    foreach (var answer in commentAnswers)
                    {
                        await _commentAnswers.CommentAnswerDeleteBL(answer);
                    }
                    await _comments.CommentDelete(comment);
                    SampleHub.BroadcastComment();
                    return RedirectToAction("PostDetail",new{ @postid = postid });
                }
            }
            else 
                return RedirectToAction("Index");
        }
        public async Task<ActionResult> CommentAnswerDelete(int? id,string UserId,int? postid)
        {
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId == UserId && id!=null)
            {
                var getAnswer = await _commentAnswers.FindPostBL(a => a.Id == id);
                await _commentAnswers.CommentAnswerDeleteBL(getAnswer);
                SampleHub.BroadcastComment();
                return RedirectToAction("PostDetail", new { @postid = postid });
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> CommentAnswerDo(int? postid,int? commentid,string commentText,CommentAnswer answer)
        {
            var comment = await _comments.FindComment(a => a.Id == commentid);
            if (comment != null)
            {
                if((postid!=null) && (commentid != null))
                {
                    string currentUserId = User.Identity.GetUserId();
                    var user = await getCurrentUser(currentUserId);
                    answer.CommentId = (int)commentid;
                    answer.UserName = user.UserName;
                    answer.UserId = currentUserId;
                    answer.AnswerDescription = commentText;
                    answer.AnswerDateTime = DateTime.Now;
                    await _commentAnswers.CommentAnswerAddBL(answer);
                    SampleHub.BroadcastComment();
                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                else
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("PostDetail", new { @postid = postid });
        }
        [AllowAnonymous]
        public async Task<ActionResult> UserProfile(string id)
        {
            string currentUserId = User.Identity.GetUserId();
            var user = await _users.Find(a => a.Id == id);
            if (user!=null)
            {
                var friend = await _userFriend.Find(a => (a.UserId1 == id && a.UserId2 == currentUserId) || (a.UserId1 == currentUserId && a.UserId2 == id));
                ProfileView model = new ProfileView()
                {
                    User = user,
                    Posts = await _posts.GetPostListOrdered(a => a.PostDateTime, b => b.UserId == id),
                    PostCount = await _posts.PostCount(a => a.UserId == id),
                    FriendsCount = await _userFriend.FriendCount(a => a.UserId1 == id || a.UserId2 == id),
                    IsFriend = friend == null ? false : true,
                    UserFriendId = friend == null ? 0 : friend.Id
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //AJAX PartialViews
        public async Task<PartialViewResult> GetComments(int postid)
        {
            DetailViewModel model = new DetailViewModel();
            var comments = await _comments.GetAll(a => a.PostId == postid);
            model.Comments = await _comments.GetCommentListOrderedDateTime(a=>a.CommentDateTime,b=>b.PostId==postid);
            model.CommentAnwers = await _commentAnswers.GetAllBL();
            model.Users = await _users.GetAll();
            model.Post = new Post {PostId = (int)postid };
            return PartialView("CommentsView",model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetPosts()
        {
            string currentId = User.Identity.GetUserId();
            ViewBag.currentUserId = currentId;
            PostViewModel model = new PostViewModel()
            {
                Posts = await _posts.GetPostListOrdered(a => a.PostDateTime),
                PostLikes = await _postLikes.PostLikeList(),
                Users = await _users.GetAll()
            };
            return PartialView("PostsView", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> ProfileGetPosts(string id) 
        {
            string currentId = User.Identity.GetUserId();
            ViewBag.currentUserId = currentId;
            PostViewModel model = new PostViewModel()
            {
                Posts = await _posts.GetPostListOrdered(a => a.PostDateTime, a => a.UserId == id),
                PostLikes = await _postLikes.PostLikeList(),
                Users = await _users.GetAll()
            };
            return PartialView("PostsView", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetLastComment()
        {
            LastCommentViewModel model = new LastCommentViewModel()
            {
                Comments = await _comments.GetCommentListOrderedIdTake(a => a.Id, 5)
            };
            return PartialView("lastComment", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetUsers(string searchKey)
        {
            string currentUserId = User.Identity.GetUserId();
            GetUserViewModel model = new GetUserViewModel()
            {
                Users = await _users.GetAll(a => a.UserName.ToLower().Contains(searchKey.ToLower()) && a.Id != currentUserId),
                UserFriends = await _userFriend.GetAll(a => (a.Check == true) && (a.UserId1 == currentUserId || a.UserId2 == currentUserId))
            };
            return PartialView("GetUsersView",model);
        }

    }
}