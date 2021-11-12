using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CommentAnswerValidator:AbstractValidator<CommentAnswer>
    {
        public CommentAnswerValidator()
        {
            RuleFor(a => a.AnswerDescription).NotEmpty().WithMessage("Cevap boş olamaz.");
            RuleFor(a => a.AnswerDescription).MaximumLength(1000).WithMessage("Karakter sınırı aşıldı.");
        }
    }
}
