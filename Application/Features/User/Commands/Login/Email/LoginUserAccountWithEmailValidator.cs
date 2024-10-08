using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Email
{
    public class LoginUserAccountValidator : AbstractValidator<LoginUserAccountWithEmailCommand>
    {
        public LoginUserAccountValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email cannot be empty");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
