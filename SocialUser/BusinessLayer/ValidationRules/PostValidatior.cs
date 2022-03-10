using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class PostValidatior : AbstractValidator<Post>
    {
        public PostValidatior()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Boş olamaz.");
            RuleFor(x => x.Description).MaximumLength(2500).WithMessage("Karakter sınırı aşıldı.");


        }
    }
}
