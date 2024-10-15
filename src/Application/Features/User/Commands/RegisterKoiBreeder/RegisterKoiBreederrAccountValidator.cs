using FluentValidation;

namespace KoiAuction.Application.User.Commands.RegisterKoiBreeder
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
        }
    }
}
