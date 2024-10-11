using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.KoiAuctionRequest.Command;
public class KoiAuctionRequestValidator : AbstractValidator<KoiAuctionRequestCommand>
{
    public KoiAuctionRequestValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.BreederId)
            .NotEmpty().WithMessage("BreederID is required");

        RuleFor(x => x.KoiId)
            .NotEmpty().WithMessage("KoiID is required");

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0).WithMessage("Initial Price must be greater than zero");

        RuleFor(x => x.AllowAutoBid)
                .NotNull().WithMessage("AllowAutoBid must be specified");

        RuleFor(x => x.AuctionMethodId)
            .NotEmpty().WithMessage("AuctionMethodID is required");

        RuleFor(x => x.IsInspectionRequired)
            .NotNull().WithMessage("Inspection Requirement must be specified");

    }
}
