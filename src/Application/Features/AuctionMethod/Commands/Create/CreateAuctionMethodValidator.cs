using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Create;
public class CreateAuctionMethodValidator : AbstractValidator<CreateAuctionMethodCommand>
{
    public CreateAuctionMethodValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("AuctionMethod Name cannot be empty");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("AuctionMethod Description cannot be empty");
    }
}
