using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EntityLayer.Concrete
{
    public class ApplicationUser : IdentityUser,IEntity
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType özelliğinin CookieAuthenticationOptions.AuthenticationType içinde tanımlanmış olanla eşleşmesi gerektiğini unutmayın
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Özel kullanıcı taleplerini buraya ekle
            return userIdentity;
        }
        [StringLength(50, ErrorMessage = "Adınız 50 karakteri geçemez")]
        [Display(Name = "Adınız")]
        public string firstName { get; set; }
        //***********************************//

        [StringLength(50, ErrorMessage = "Soyadınız 50 karakteri geçemez")]
        [Display(Name = "Soyadınız")]
        public string lastName { get; set; }

        //***********************************//
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{dd.MM.yyyy}")]
        [Required]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DateOfBirth { get; set; }


        //***********************************//

        [Display(Name = "Profil Fotoğrafı")]
        public string profilePhoto { get; set; }

        [Display(Name = "Profil Açıklaması")]
        public string profileDescription { get; set; }
        //***********************************//
    }
}
