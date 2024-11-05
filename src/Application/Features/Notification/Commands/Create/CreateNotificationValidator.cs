using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using FluentValidation;

namespace Application.Features.Notification.Commands.Create;
public class CreateNotificationValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message cannot be empty.");
    }
}
