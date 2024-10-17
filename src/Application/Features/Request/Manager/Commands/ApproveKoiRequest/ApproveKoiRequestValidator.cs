using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Commands.ApproveRoleRequest;
using FluentValidation;

namespace Application.Features.Request.Manager.Commands.ApproveKoiRequest;
public class ApproveKoiRequestValidator : AbstractValidator<ApproveKoiRequestCommand>
{
    public ApproveKoiRequestValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.KoiID)
          .NotEmpty().WithMessage("KoiID cannot be empty!");
    }
}
