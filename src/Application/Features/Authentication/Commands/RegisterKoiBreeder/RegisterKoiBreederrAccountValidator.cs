using FluentValidation;

namespace Application.Features.Authentication.Commands.RegisterKoiBreeder
{
    public class RegisterKoiBreederrAccountValidator : AbstractValidator<RegisterKoiBreederAccountCommand>
    {
        public RegisterKoiBreederrAccountValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.KoiFarmName)
                .NotEmpty().WithMessage("Koi Farm Name cannot be empty")
                .MaximumLength(50).WithMessage("Koi Farm Name cannot exceed 50 characters");

            RuleFor(x => x.KoiFarmDescription)
                .NotEmpty().WithMessage("You should provide a brief description of your farm.");

            RuleFor(x => x.KoiFarmLocation)
                .NotEmpty().WithMessage("You should provide location address of your farm.");

            RuleFor(x => x.KoiFarmImage)
                .NotEmpty().WithMessage("KoiFarmImage cannot be empty.");
        }
    }
}
