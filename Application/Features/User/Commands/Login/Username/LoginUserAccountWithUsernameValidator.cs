using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Username
{
    public class LoginUserAccountWithUsernameValidator : AbstractValidator<LoginUserAccountWithUsernameCommand>
    {
        public LoginUserAccountWithUsernameValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.Username)
               .NotEmpty().WithMessage("Username cannot be empty");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
