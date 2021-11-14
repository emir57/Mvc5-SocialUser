using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(a => a.CommentDescription).NotEmpty().WithMessage("Yorum boş olamaz.");
            RuleFor(a => a.CommentDescription).MaximumLength(1000).WithMessage("Karakter sınırı aşıldı.");
        }
    }
}
