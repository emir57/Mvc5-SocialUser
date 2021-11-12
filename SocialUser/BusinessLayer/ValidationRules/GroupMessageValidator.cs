using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class GroupMessageValidator:AbstractValidator<GroupMessage>
    {
        public GroupMessageValidator()
        {
            RuleFor(g => g.Message).NotNull().NotEmpty().WithMessage("Mesaj Boş olamaz");
            RuleFor(g => g.Message).MaximumLength(5000).WithMessage("Karakter sınırı aşıldı");
            //RuleFor(g => g.Message).MinimumLength(1).WithMessage("Minimum 1 karakter olabilir");
        }
    }
}
