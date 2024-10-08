using FluentValidation;
using KoiAuction.Application.Features.Auction.Commands.Create;

namespace AuctionAuction.Application.Features.Auction.Commands.Create
{
    public class CreateAuctionValidator : AbstractValidator<CreateAuctionCommand>
    {
        public CreateAuctionValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            
        }
    }
}
