using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KoiAuction.Application.Features.User.Commands.Login.Email;

namespace Application.Features.Request.Manager.Commands.DenyRoleRequest;
public class DenyRoleRequestValidator : AbstractValidator<DenyRoleRequestCommand>
{
    public DenyRoleRequestValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.KoiBreederID)
          .NotEmpty().WithMessage("KoiBreederID cannot be empty!");

        RuleFor(x => x.RequestResponse)
           .NotEmpty().WithMessage("You should write the reason to deny!");
    }
}
