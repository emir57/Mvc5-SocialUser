using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CommentAnswerValidatior:AbstractValidator<CommentAnswer>
    {
        public CommentAnswerValidatior()
        {
            RuleFor(a => a.AnswerDescription).NotEmpty().WithMessage("Cevap boş olamaz.");
            RuleFor(a => a.AnswerDescription).MaximumLength(1000).WithMessage("Karakter sınırı aşıldı.");
        }
    }
}
