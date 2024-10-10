using FluentValidation;
using KoiAuction.Application.Features.AutoBid.Commands.Create;

namespace KoiAuction.Application.Features.AutoBid.Commands.Create
{
    public class CreateAutoBidValidator : AbstractValidator<CreateAutoBidCommand>
    {
        public CreateAutoBidValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            
        }
    }
}
