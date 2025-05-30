using FluentValidation;
using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.ValidationRules
{
    public class RolValidator:AbstractValidator<Rol>
    {
        public RolValidator()
        {
            RuleFor(x => x.RolAdi)
                    .NotEmpty().WithMessage("Rol adı zorunludur.")
                    .MaximumLength(50).WithMessage("Rol adı en fazla 50 karakter olabilir.")
                    .MinimumLength(4).WithMessage("Rol adı en az 4 karakter olmalıdır.")
                    .Matches("^[a-zA-ZğüşöçıİĞÜŞÖÇ ]+$")
                    .WithMessage("Rol adı sadece harflerden oluşmalıdır.");
        }
    }
}
