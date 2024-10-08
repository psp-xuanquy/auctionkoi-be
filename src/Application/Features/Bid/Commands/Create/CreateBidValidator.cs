using FluentValidation;
using KoiAuction.Application.Features.Bid.Commands.Create;

namespace KoiAuction.Application.Features.Bid.Commands.Create
{
    public class CreateBidValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            
        }
    }
}
