using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Commands.Create;
using FluentValidation;

namespace Application.Features.Koi.Commands.Update;
public class UpdateKoiValidator : AbstractValidator<UpdateKoiCommand>
{
    public UpdateKoiValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Koi Name cannot be empty");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Koi Description cannot be empty");

        RuleFor(x => x.InitialPrice)
            .GreaterThanOrEqualTo(50).WithMessage("Initial Price must be at least $50");

        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Size must be greater than 0");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be greater than 0");
    }
}
