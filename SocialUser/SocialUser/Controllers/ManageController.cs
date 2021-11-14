using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SocialUser.Models;
using System.IO;
using System.Data.Entity;
using EntityLayer.Concrete;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Abstract;
using BusinessLayer.Utilities;
using SocialUser.Utilities;

namespace SocialUser.Controllers
{
    
    [Authorize]
    public class ManageController : Controller
    {
        private IUserService _userService = NinjectInstanceFactory.GetInstance<IUserService>();
        private IUserFriendService _userFriendService = NinjectInstanceFactory.GetInstance<IUserFriendService>();

        

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Parolanız değiştirildi."
                : message == ManageMessageId.SetPasswordSuccess ? "Parolanız ayarlandı."
                : message == ManageMessageId.SetTwoFactorSuccess ? "İki öğeli kimlik doğrulama sağlayıcısı ayarlandı."
                : message == ManageMessageId.Error ? "Hata oluştu."
                : message == ManageMessageId.AddPhoneSuccess ? "Telefon numaranız eklendi."
                : message == ManageMessageId.RemovePhoneSuccess ? "Telefon numaranız kaldırıldı."
                : "";

            string userId = User.Identity.GetUserId();
            var currentUser = await UserUtility.GetCurrentUser(userId);
            ViewData["photoUrl"] = currentUser.profilePhoto;
            ViewData["description"] = currentUser.profileDescription;
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),

                //Friends
                userFriends = await _userFriendService.GetAll(),
                users = await _userService.GetAll()
        };

            return View(model);
        }
        public async Task<ActionResult> Accept(int? id)
        {
            var result = await _userFriendService.Find(a => a.Id == id);
            result.Check = true;
            var userid1 = result.UserId1;
            var userid2 = result.UserId2;
            await _userFriendService.Update(result);
            SampleHub.BroadcastFriend(userid1, userid2);
            
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            var result = await _userFriendService.Find(a => a.Id == id);
            var userid1 = result.UserId1;
            var userid2 = result.UserId2;
            await _userFriendService.Delete(result);
            SampleHub.BroadcastFriend(userid1, userid2);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Send(string username, UserFriend userFriend)
        {
            //Userid1 currentuser
            IndexViewModel model = new IndexViewModel();
            string currentUserId = User.Identity.GetUserId();

            var user = await _userService.Find(a => a.UserName == username);

            //check
            var userFriendCheck = await _userFriendService.Find(a => (a.UserId1 == currentUserId && a.UserId2 == user.Id) || (a.UserId2 == currentUserId && a.UserId1 == user.Id));
            if (userFriendCheck == null)
            {
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var userCheck = _userFriendService.Find(a => a.UserId1 == currentUserId && a.UserId2 == user.Id);
                    if (userCheck != null)
                    {
                        userFriend.Check = false;
                        userFriend.UserId1 = currentUserId;
                        userFriend.UserId2 = user.Id;
                        await _userFriendService.Add(userFriend);
                        SampleHub.BroadcastFriend(currentUserId, user.Id);
                        return RedirectToAction("Index");
                    }

                }
            }
            else 
            { 
                return RedirectToAction("Index"); 
            }

           
            return RedirectToAction("Index");
        }

        public async Task<PartialViewResult> GetFriendList()
        {
            IndexViewModel model = new IndexViewModel();
            model.userFriends = await _userFriendService.GetAll();
            model.users = await _userService.GetAll();

            //friends Check
            string userId = User.Identity.GetUserId();
            var friends = await _userFriendService.GetAll(a => (a.Check == true) && ((a.UserId1 == userId) || (a.UserId2 == userId)));
            var friendsRequest = await _userFriendService.GetAll(a => (a.Check == false) && ((a.UserId1 == userId) || a.UserId2 == userId));
            if (friends.Count == 0)
            {
                model.friends = "Henüz arkadaşınız yok.";
            }
            if (friendsRequest.Count == 0)
            {
                model.friendsRequest = "Arkadaşlık isteğiniz yok.";
            }

            return PartialView("FriendsList",model);
        }
        public ActionResult SetProfilePhoto()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> SetProfilePhoto(int?id)
        {
            string currentUserId = User.Identity.GetUserId();
            var currentUser = await UserUtility.GetCurrentUser(currentUserId);
            ViewBag.photo = currentUser.profilePhoto;

            return View();
        }

        public async Task<ActionResult> SetProfileDescription(string description)
        {
            if(!string.IsNullOrWhiteSpace(description))
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUser = await UserUtility.GetCurrentUser(currentUserId);
                currentUser.profileDescription = description;
                await _userService.UpdateUser(currentUser);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetProfilePhoto(HttpPostedFileBase picture)
        {
            string currentUserId = User.Identity.GetUserId();
            var currentUser = await UserUtility.GetCurrentUser(currentUserId);
            string databasePath ="";
            if (picture.ContentLength >= 0)
            {
                //delete old photo
                string oldFileName = currentUser.profilePhoto.Split('/')[4];
                if (oldFileName != "person.jpg")
                {
                    string oldPath = Server.MapPath("~/Content/profilePhoto/" + oldFileName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                
                //save new photo
                string getEx = Path.GetExtension(picture.FileName);
                string filename = Guid.NewGuid() + getEx;
                string path = Server.MapPath("~/Content/profilePhoto/");
                databasePath = "../../Content/profilePhoto/" + filename;
                picture.SaveAs(Path.Combine(path, filename));
            }
            else
            {
                ViewBag.message = "Fotoğraf Seçilemedi!";
                return View();
            }
            
            currentUser.profilePhoto = databasePath;
            await _userService.UpdateUser(currentUser);
            SampleHub.BroadcastPost();
            return RedirectToAction("SetProfilePhoto");
            
        }

        [HttpPost]
        public async Task<ActionResult> RemoveProfilePhoto()
        {
            string currentUserId = User.Identity.GetUserId();
            var currentUser = await UserUtility.GetCurrentUser(currentUserId);
            //old delete photo
            string oldFileName = currentUser.profilePhoto.Split('/')[4];
            if (oldFileName != "person.jpg") 
            {
                string oldPath = Server.MapPath("~/Content/profilePhoto/" + oldFileName);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            
            
            //set default photo
            currentUser.profilePhoto = "../../Content/profilePhoto/person.jpg";
            await _userService.UpdateUser(currentUser);
            SampleHub.BroadcastPost();
            return RedirectToAction("Index");
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Belirteç oluştur ve gönder
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Güvenlik kodunuz: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Telefon numaranızı doğrulamak için SMS sağlayıcınız üzerinden SMS gönderin
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Bir hata oluştu, formu yeniden görüntüleyin
            ModelState.AddModelError("", "Telefon numarası doğrulanamadı");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Bir hata oluştu, formu yeniden görüntüleyin
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Dış oturum kaldırıldı."
                : message == ManageMessageId.Error ? "Hata oluştu."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Oturumun geçerli kullanıcıya bağlaması için dış oturum sağlayıcısının yeniden yönlendirilmesini isteyin
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Yardımcılar
        // Dış oturumlar eklenirken XSRF koruması için kullanıldı
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}