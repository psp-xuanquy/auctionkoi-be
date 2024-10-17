using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Commands.DenyRoleRequest;
using FluentValidation;

namespace Application.Features.Request.Manager.Commands.DenyKoiRequest;
public class DenyKoiRequestValidator : AbstractValidator<DenyKoiRequestCommand>
{
    public DenyKoiRequestValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.KoiID)
          .NotEmpty().WithMessage("KoiID cannot be empty!");

        RuleFor(x => x.RequestResponse)
           .NotEmpty().WithMessage("You should write the reason to deny!");
    }
}
