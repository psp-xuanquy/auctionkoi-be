using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KoiAuction.Application.User.Commands.RegisterCustomer;

namespace Application.Features.Authentication.Commands.RegisterStaff;
public class RegisterStaffAccountValidator : AbstractValidator<RegisterStaffAccountCommand>
{
    public RegisterStaffAccountValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .Matches(@"^[a-zA-Z0-9._%+-]+@staff\.com$").WithMessage("Email format is incorrect (Expected format: example@staff.com)");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least 1 uppercase letter")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least 1 special character");

        //RuleFor(x => x.UserName)
        //    .NotEmpty().WithMessage("Username cannot be empty")
        //    .Length(4, 20).WithMessage("Username should be longer than 4 and shorter than 20 characters");

        //RuleFor(x => x.FullName)
        //    .NotEmpty().WithMessage("Full name cannot be empty")
        //    .MaximumLength(50).WithMessage("Full name cannot exceed 50 characters");

        //RuleFor(x => x.Gender)
        //    .NotEmpty().WithMessage("Gender cannot be empty")
        //    .Must(g => g == "Male" || g == "Female" || g == "Other")
        //    .WithMessage("Please select one of the three genders: 'Male', 'Female', or 'Other'");

        //RuleFor(x => x.PhoneNumber)
        //   .NotEmpty().WithMessage("PhoneNumber cannot be empty")
        //   .Matches(@"^\+?[0-9]{10,15}$").WithMessage("PhoneNumber is not valid");

        //RuleFor(x => x.Address)
        //   .NotEmpty().WithMessage("Address cannot be empty");
    }
}

