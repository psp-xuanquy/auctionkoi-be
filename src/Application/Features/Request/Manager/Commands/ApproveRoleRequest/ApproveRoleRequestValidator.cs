using FluentValidation;

namespace Application.Features.Request.Manager.Commands.ApproveRoleRequest;
public class ApproveRoleRequestValidator : AbstractValidator<ApproveRoleRequestCommand>
{
    public ApproveRoleRequestValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.KoiBreederID)
          .NotEmpty().WithMessage("KoiBreederID cannot be empty!");
    }
}
