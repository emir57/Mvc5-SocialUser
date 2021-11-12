using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ChatMessageValidator:AbstractValidator<ChatMessage>
    {
        public ChatMessageValidator()
        {
            RuleFor(g => g.MessageText).NotNull().NotEmpty().WithMessage("Mesaj Boş olamaz");
            RuleFor(g => g.MessageText).MaximumLength(5000).WithMessage("Karakter sınırı aşıldı");
        }
    }
}
