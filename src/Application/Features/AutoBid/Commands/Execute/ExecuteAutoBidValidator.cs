using FluentValidation;
using KoiAuction.Application.Features.AutoBid.Commands.Create;

namespace KoiAuction.Application.Features.AutoBid.Commands.Execute
{
    public class ExecuteAutoBidValidator : AbstractValidator<ExecuteAutoBidCommand>
    {
        public ExecuteAutoBidValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            
        }
    }
}
