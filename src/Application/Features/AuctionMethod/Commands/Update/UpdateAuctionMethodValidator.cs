using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Commands.Create;
using FluentValidation;

namespace Application.Features.AuctionMethod.Commands.Update;
public class UpdateAuctionMethodValidator : AbstractValidator<UpdateAuctionMethodCommand>
{
    public UpdateAuctionMethodValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        //RuleFor(x => x.Id)
        //   .NotEmpty().WithMessage("AuctionMethod Id cannot be empty");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("AuctionMethod Name cannot be empty");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("AuctionMethod Description cannot be empty");
    }
}
