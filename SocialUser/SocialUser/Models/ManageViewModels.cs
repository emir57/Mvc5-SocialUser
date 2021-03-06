using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityLayer.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SocialUser.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public List<UserFriend> userFriends { get; set; }
        public List<ApplicationUser> users { get; set; }
        public string friends { get; set; }
        public string friendsRequest { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parola")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni parolayı onaylayın")]
        [Compare("NewPassword", ErrorMessage = "Yeni parola ve onay parolası eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
    public class SetProfilePhoto
    {

        [Display(Name = "Profil Fotoğrafı")]
        public string profilePhoto { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut parola")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parola")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni parolayı onaylayın")]
        [Compare("NewPassword", ErrorMessage = "Yeni parola ve onay parolası eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}