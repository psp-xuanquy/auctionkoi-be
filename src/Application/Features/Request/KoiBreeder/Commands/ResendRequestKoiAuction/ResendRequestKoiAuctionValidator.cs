using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using FluentValidation;
using KoiAuction.Domain.Enums;

namespace Application.Features.Request.KoiBreeder.Commands.ResendRequestKoiAuction;
public class ResendRequestKoiAuctionValidator : AbstractValidator<ResendRequestKoiAuctionCommand>
{
    public ResendRequestKoiAuctionValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Koi Name cannot be empty.")
            .MaximumLength(50).WithMessage("Koi Name cannot exceed 50 characters");

        RuleFor(x => x.Sex)
            .NotEmpty().WithMessage("Sex cannot be empty.")
            .Must(g => g == "Male" || g == "Female")
            .WithMessage("Please select valid Sex for Koi: 'Male' or 'Female'");

        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size cannot be empty.")
            .GreaterThanOrEqualTo(10).WithMessage("Size must be at least 10 cm.")
            .LessThanOrEqualTo(100).WithMessage("Size cannot exceed 100 cm.");

        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("Age cannot be empty.")
            .GreaterThanOrEqualTo(1).WithMessage("Age must be at least 1 year.")
            .LessThanOrEqualTo(20).WithMessage("Age cannot exceed 20 years.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location cannot be empty.");

        RuleFor(x => x.Variety)
            .NotEmpty().WithMessage("Variety cannot be empty.")
            .Must(BeAValidVariety).WithMessage("Invalid Variety. Must be one of: " + string.Join(", ", Enum.GetNames(typeof(Variety))));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("You should provide a brief description of your Koi.");

        RuleFor(x => x.StartTime)
            .Must(BeAValidStartTime).WithMessage("StartTime cannot be in the past and must be at least 5 minutes from the current time.");

        RuleFor(x => x.InitialPrice)
           .NotEmpty().WithMessage("InitialPrice cannot be empty.");

        RuleFor(x => x.AuctionMethodID)
            .NotEmpty().WithMessage("AuctionMethod cannot be empty.");
    }


    private bool BeAValidVariety(string variety)
    {
        return Enum.TryParse(typeof(Variety), variety, true, out _);
    }

    // Kiểm tra xem StartTime có cách thời gian hiện tại ít nhất 5 phút không
    private bool BeAValidStartTime(DateTime? startTime)
    {
        var currentTime = DateTime.Now;
        return startTime > currentTime.AddMinutes(1);
    }
}
