using FluentValidation;

namespace Application.Features.Bid.FixedPriceBid
{
    public class PlaceFixedPriceBidValidator : AbstractValidator<PlaceFixedPriceBidCommand>
    {
        public PlaceFixedPriceBidValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.KoiId)
                .NotEmpty().WithMessage("KoiId cannot be empty.");

            RuleFor(x => x.BidAmount)
                .GreaterThanOrEqualTo(50).WithMessage("Bid amount must be at least $50.");
        }
    }
}
