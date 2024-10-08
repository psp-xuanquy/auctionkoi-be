using FluentValidation;

namespace AuctionAuction.Application.Features.Auction.Commands.Update
{
    public class UpdateAuctionValidator : AbstractValidator<UpdateAuctionCommand>
    {
        public UpdateAuctionValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            
        }
    }
}
