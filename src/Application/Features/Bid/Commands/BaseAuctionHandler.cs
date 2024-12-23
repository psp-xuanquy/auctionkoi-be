﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;

namespace Application.Features.Bid.Commands;
public abstract class BaseAuctionHandler
{
    protected readonly IKoiRepository _koiRepository;
    protected readonly IBidRepository _bidRepository;
    protected readonly IAutoBidRepository _autoBidRepository;
    protected readonly IUserRepository _userRepository;
    protected readonly ICurrentUserService _currentUserService;

    protected BaseAuctionHandler(
        IKoiRepository koiRepository,
        IBidRepository bidRepository,
        IAutoBidRepository autoBidRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _koiRepository = koiRepository;
        _bidRepository = bidRepository;
        _autoBidRepository = autoBidRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    protected async Task<UserEntity> GetCurrentBidder(CancellationToken cancellationToken)
    {
        var bidder = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
        if (bidder == null)
        {
            throw new NotFoundException("Please login again");
        }
        return bidder;
    }

    protected async Task<KoiEntity> GetKoiForAuction(string koiId, string auctionMethod, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(k => k.ID == koiId, cancellationToken);
        
        if (koi == null)
        {
            throw new NotFoundException($"Koi with ID '{koiId}' not found for auction method '{auctionMethod}'.");
        }
        if (koi.AuctionStatus != AuctionStatus.OnGoing)
        {
            throw new Exception("Auction for this Koi is not active.");
        }
        return koi;
    }

    protected async Task ValidateBidAmount(decimal bidAmount, string koiId, string bidderId, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(k => k.ID == koiId
            && k.AuctionStatus == AuctionStatus.OnGoing
            , cancellationToken);
        
        if (koi == null)
            throw new NotFoundException($"Koi with ID '{koiId}' not found.");

        var bidder = await _userRepository.FindAsync(b => b.Id == bidderId, cancellationToken);
        if (bidder == null)
            throw new NotFoundException("Bidder not found.");

        //if (bidAmount < koi.InitialPrice)
        //    throw new Exception($"Bid amount must be greater than the starting price of {koi.InitialPrice:C}.");

        if (bidAmount > bidder.Balance)
            throw new Exception("Bid amount exceeds available balance.");

        //var existingBid = await _bidRepository.GetUserBidForKoi(koiId, bidderId, cancellationToken);
        //if (existingBid != null)
        //    throw new Exception("You have already placed a bid for this auction.");
    }

}
