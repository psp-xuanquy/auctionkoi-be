using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Wallet.Deposit;
public class DepositToWalletValidator : AbstractValidator<DepositToWalletCommand>
{
    public DepositToWalletValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.DepositAmount)
              .NotEmpty().WithMessage("DepositAmount cannot be Empty")
              .GreaterThanOrEqualTo(10000).WithMessage("The minimum deposit amount must be 10000VNĐ");
    }
}
