using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CommentValidatior: AbstractValidator<Comment>
    {
        public CommentValidatior()
        {
            RuleFor(a => a.CommentDescription).NotEmpty().WithMessage("Yorum boş olamaz.");
            RuleFor(a => a.CommentDescription).MaximumLength(1000).WithMessage("Karakter sınırı aşıldı.");
        }
    }
}
