using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using FluentValidation;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.SealedBidAuction;

public class PlaceSealedBidAuctionValidator : AbstractValidator<PlaceSealedBidAuctionCommand>
{
    public PlaceSealedBidAuctionValidator()
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
