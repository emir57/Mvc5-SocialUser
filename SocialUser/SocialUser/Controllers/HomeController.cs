using BusinessLayer.Abstract;
using BusinessLayer.Utilities;
using EntityLayer.Concrete;
using Microsoft.AspNet.Identity;
using SocialUser.Models;
using SocialUser.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialUser.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IPostService _postService;
        private ICommentService _commentService;
        private ICommentAnswerService _commentAnswerService;
        private IPostLikeService _postLikeService;
        private IUserService _userService;
        private IUserFriendService _userFriendService;
        public HomeController(IUserFriendService userFriendService, IUserService userService, IPostLikeService postLikeService, ICommentAnswerService commentAnswerService, ICommentService commentService, IPostService postService)
        {
            _userFriendService = userFriendService;
            _userService = userService;
            _postLikeService = postLikeService;
            _commentAnswerService = commentAnswerService;
            _commentService = commentService;
            _postService = postService;
        }

        public string PicturePath()
        {
            return Server.MapPath("~/Content/postPicture/");
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
        public async Task<ActionResult> PostAdd(string description, HttpPostedFileBase picture, Post post)
        {
            if (User.Identity.IsAuthenticated)
            {
                string databasePath = "";
                string currentId = User.Identity.GetUserId();
                var currentUser = await UserUtility.GetCurrentUser(currentId);

                if (picture != null)
                {
                    string path = PicturePath();
                    ImageUtility.UploadImage(picture, out databasePath, path);
                }
                //add post
                post.Description = description;
                post.LikeCount = 0;
                post.PostPicture = databasePath;
                post.UserProfilePhoto = currentUser.profilePhoto;
                post.Username = currentUser.UserName;
                post.UserId = currentUser.Id;
                post.PostDateTime = DateTime.Now;
                await _postService.PostAdd(post);

                SocialUserSignalRHub.BroadcastPost();
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> PostDelete(int id)
        {

            var post = await _postService.FindPost(a => a.PostId == id);

            var comments = await _commentService.GetAll(a => a.PostId == id);

            var commentAnswers = await _commentAnswerService.GetAll();

            //delete post picture
            if (!(String.IsNullOrEmpty(post.PostPicture)))
            {
                string[] pictureName = post.PostPicture.Split('/');
                
                string fullPath = PicturePath() + pictureName[4];
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            await _postService.PostDelete(post);

            //delete comments and answers
            foreach (var comment in comments)
            {
                foreach (var answers in commentAnswers)
                {
                    if (answers.CommentId == comment.Id)
                    {
                        await _commentAnswerService.Delete(answers);
                    }
                }
                await _commentService.CommentDelete(comment);
            }

            SocialUserSignalRHub.BroadcastPost();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> PostDetail(int? postid)
        {
            var post = await _postService.FindPost(a => a.PostId == postid);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string currentUserId = User.Identity.GetUserId();
                //do current user like post? null=not like
                var search = await _postLikeService.PostLikeFind(a => a.UserId == currentUserId && a.PostId == postid);
                if (search == null)
                {
                    //user dont like = false
                    //user like = true
                    ViewData["checkLike"] = false;
                }
                else { ViewData["checkLike"] = true; }

                //get sharing user
                var postUserId = post.UserId;
                var user = await _userService.Find(a => a.Id == postUserId);

                //post info


                DetailViewModel model = new DetailViewModel();
                model.Post = post;
                model.comment = await _commentService.GetAll(a => a.PostId == post.PostId);
                model.comment = await _commentService.GetCommentListOrderedDateTime(a => a.CommentDateTime);
                model.commentAnswer = await _commentAnswerService.GetAll(a => a.CommentId == postid);
                model.likes = await _postLikeService.PostLikeList(a => a.PostId == postid);
                model.users = await _userService.GetAll();
                model.User = user;
                return View(model);
            }
        }

        public async Task<ActionResult> PostLike(int? postid, bool? check)
        {
            if (User.Identity.IsAuthenticated)
            {
                PostLike like = new PostLike();
                string currentUserId = User.Identity.GetUserId();
                if (postid != null && check == false)
                {
                    //like
                    like.UserId = currentUserId;
                    like.PostId = (int)postid;
                    await _postLikeService.PostLikeAdd(like);

                    var likes = await _postLikeService.PostLikeList(a => a.PostId == postid);
                    int likecount = likes.Count();

                    await PostUtility.UpdateLikeCount((int)postid, likecount);
                    SocialUserSignalRHub.BroadcastPost();

                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                else if (postid != null && check == true)
                {
                    //like delete
                    var search = await _postLikeService.PostLikeFind(a => a.PostId == postid && a.UserId == currentUserId);
                    await _postLikeService.PostLikeDelete(search);

                    var likes = await _postLikeService.PostLikeList(a => a.PostId == postid);
                    int likecount = likes.Count();

                    //update post like count
                    await PostUtility.UpdateLikeCount((int)postid, likecount);
                    SocialUserSignalRHub.BroadcastPost();

                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostDoComment(int? postid, string text, Comment comment)
        {
            var post = await _postService.FindPost(a => a.PostId == postid);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var user = await UserUtility.GetCurrentUser(currentUserId);

                    comment.PostId = post.PostId;
                    comment.UserName = user.UserName;
                    comment.UserId = user.Id;
                    comment.CommentDescription = text;
                    comment.CommentDateTime = DateTime.Now;

                    await _commentService.CommentAdd(comment);

                    SocialUserSignalRHub.BroadcastComment();
                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                else
                {
                    return RedirectToAction("PostDetail", postid);
                }
            }
        }
        public async Task<ActionResult> PostCommentDelete(int? id, int postid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var comment = await _commentService.FindComment(a => a.Id == id);
                var commentAnswers = await _commentAnswerService.GetAll(a => a.CommentId == comment.Id);
                if (comment == null) { return RedirectToAction("Index"); }
                else
                {

                    foreach (var answer in commentAnswers)
                    {
                        await _commentAnswerService.Delete(answer);
                    }
                    await _commentService.CommentDelete(comment);

                    SocialUserSignalRHub.BroadcastComment();
                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
            }
            else { return RedirectToAction("Index"); }
        }
        public async Task<ActionResult> CommentAnswerDelete(int? id, string UserId, int? postid)
        {
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId == UserId && id != null)
            {
                var getAnswer = await _commentAnswerService.FindPost(a => a.Id == id);
                await _commentAnswerService.Delete(getAnswer);
                SocialUserSignalRHub.BroadcastComment();
                return RedirectToAction("PostDetail", new { @postid = postid });

            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CommentAnswerDo(int? postid, int? commentid, string commentText, CommentAnswer answer)
        {
            var comment = await _commentService.FindComment(a => a.Id == commentid);
            if (comment != null)
            {
                if ((postid != null) && (commentid != null))
                {
                    string currentUserId = User.Identity.GetUserId();
                    var user = await UserUtility.GetCurrentUser(currentUserId);
                    answer.CommentId = (int)commentid;
                    answer.UserName = user.UserName;
                    answer.UserId = currentUserId;
                    answer.AnswerDescription = commentText;
                    answer.AnswerDateTime = DateTime.Now;
                    await _commentAnswerService.Add(answer);
                    SocialUserSignalRHub.BroadcastComment();
                    return RedirectToAction("PostDetail", new { @postid = postid });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("PostDetail", new { @postid = postid });
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> UserProfile(string id)
        {
            ProfileView model = new ProfileView();
            string currentUserId = User.Identity.GetUserId();

            var user = await _userService.Find(a => a.Id == id);
            if (user != null)
            {
                model.user = user;

                var posts = await _postService.GetPostListOrdered(a => a.PostDateTime, b => b.UserId == id);
                model.posts = posts;

                model.postCount = await _postService.PostCount(a => a.UserId == id);
                model.friendsCount = await _userFriendService.FriendCount(a => a.UserId1 == id || a.UserId2 == id);
                var friend = await _userFriendService.Find(a => (a.UserId1 == id && a.UserId2 == currentUserId) || (a.UserId1 == currentUserId && a.UserId2 == id));
                if (friend == null)
                {
                    model.isFriend = false;
                }
                else
                {
                    model.isFriend = true;
                    model.userFriendId = friend.Id;

                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //********************************************************************
        //AJAX PartialViews
        public async Task<PartialViewResult> GetComments(int? postid)
        {
            DetailViewModel model = new DetailViewModel();
            var comments = await _commentService.GetAll(a => a.PostId == postid);
            model.comment = await _commentService.GetCommentListOrderedDateTime(a => a.CommentDateTime, b => b.PostId == postid);
            model.commentAnswer = await _commentAnswerService.GetAll();
            model.users = await _userService.GetAll();
            model.postid = (int)postid;
            return PartialView("CommentsView", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetPosts()
        {
            string currentId = User.Identity.GetUserId();
            ViewBag.currentUserId = currentId;
            PostViewModel model = new PostViewModel();

            model.posts = await _postService.GetPostListOrdered(a=>a.PostDateTime);
            model.postLike = await _postLikeService.PostLikeList();
            model.users = await _userService.GetAll();
            return PartialView("PostsView", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> ProfileGetPosts(string id)
        {
            string currentId = User.Identity.GetUserId();
            ViewBag.currentUserId = currentId;
            PostViewModel model = new PostViewModel();

            model.posts = await _postService.GetPostListOrdered(a => a.PostDateTime, a => a.UserId == id);
            model.postLike = await _postLikeService.PostLikeList();
            model.users = await _userService.GetAll();
            return PartialView("PostsView", model);

        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetLastComment()
        {
            LastCommentViewModel model = new LastCommentViewModel();
            model.comments = await _commentService.GetCommentListOrderedIdTake(a => a.Id, 5);
            return PartialView("lastComment", model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> GetUsers(string searchKey)
        {
            GetUserViewModel model = new GetUserViewModel();
            string currentUserId = User.Identity.GetUserId();
            //get search users
            model.users = await _userService.GetAll(a => a.UserName.ToLower().Contains(searchKey.ToLower()) && a.Id != currentUserId);
            //get current user friend
            model.userFriend = await _userFriendService.GetAll(a => (a.CheckFriend == true) && (a.UserId1 == currentUserId || a.UserId2 == currentUserId));
            return PartialView("GetUsersView", model);
        }

    }
}