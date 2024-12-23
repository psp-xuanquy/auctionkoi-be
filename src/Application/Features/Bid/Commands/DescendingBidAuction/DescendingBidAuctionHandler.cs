﻿using Application.Common.Exceptions;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.Commands.DescendingBidAuction;
public class DescendingBidAuctionHandler : BaseAuctionHandler, IRequestHandler<DescendingBidAuctionCommand, string>
{
    public DescendingBidAuctionHandler(
        IKoiRepository koiRepository,
        IBidRepository bidRepository,
        IAutoBidRepository autoBidRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
        : base(koiRepository, bidRepository, autoBidRepository, userRepository, currentUserService)
    {
    }

    public async Task<string> Handle(DescendingBidAuctionCommand request, CancellationToken cancellationToken)
    {
        var bidder = await GetCurrentBidder(cancellationToken);
        var koi = await GetKoiForAuction(request.KoiId, "Descending Bid Auction", cancellationToken);

        await ValidateBid(request, bidder, koi, cancellationToken);

        var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice);
        bid.IsWinningBid = true;

        _bidRepository.Add(bid);

        if (koi.AuctionStatus == AuctionStatus.OnGoing)
        {
            koi.EndAuction();
            _koiRepository.Update(koi);
        }

        await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return $"You have successfully bid {request.BidAmount} for the fish {koi.Name}.";
    }

    private async Task ValidateBid(DescendingBidAuctionCommand request, UserEntity bidder, KoiEntity koi, CancellationToken cancellationToken)
    {
        if (request.BidAmount > bidder.Balance)
            throw new InvalidOperationException("Bid amount cannot exceed the user's balance.");

        var existingBid = await _bidRepository.GetUserBidForKoi(koi.ID, bidder.Id, cancellationToken);
        if (existingBid != null)
            throw new BadRequestException("You have already placed a bid for this auction.");

        await ValidateBidAmount(request.BidAmount, koi.ID, bidder.Id, cancellationToken);

        if (koi.CurrentDescendedPrice == null || koi.CurrentDescendedPrice == 0)
        {
            if (koi.InitialPrice != request.BidAmount)
            {
                throw new InvalidOperationException("You must accept the current price to bid.");
            }
        }
        else
        {
            if (koi.CurrentDescendedPrice != request.BidAmount)
            {
                throw new InvalidOperationException("You must accept the current price to bid.");
            }
        }

        if (koi.IsAuctionExpired())
        {
            throw new InvalidOperationException("The auction has already expired.");
        }
    }
}
