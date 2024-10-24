﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;

namespace Application.Features.Bid;
public abstract class BaseAuctionHandler
{
    protected readonly IKoiRepository _koiRepository;
    protected readonly IBidRepository _bidRepository;
    protected readonly IUserRepository _userRepository;
    protected readonly ICurrentUserService _currentUserService;

    protected BaseAuctionHandler(
        IKoiRepository koiRepository,
        IBidRepository bidRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _koiRepository = koiRepository;
        _bidRepository = bidRepository;
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
        var koi = await _koiRepository.FindAsync(k => k.ID == koiId && k.AuctionMethod.Name == auctionMethod && k.DeletedTime == null, cancellationToken);
        if (koi == null || koi.AuctionStatus != AuctionStatus.OnGoing)
        {
            throw new InvalidOperationException("Auction for this Koi is not active.");
        }
        return koi;
    }

    protected void ValidateBidAmount(decimal bidAmount, KoiEntity koi, UserEntity bidder)
    {
        if (bidAmount <= koi.InitialPrice)
            throw new InvalidOperationException("Bid amount must be greater than the starting price.");
        if (bidAmount > bidder.Balance)
            throw new InvalidOperationException("Bid amount exceeds available balance.");
    }
}
