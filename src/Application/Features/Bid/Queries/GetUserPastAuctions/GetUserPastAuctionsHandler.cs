using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Koi;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Common.Exceptions;
using MediatR;
using Domain.Enums;
using KoiAuction.Domain.Enums;

namespace Application.Features.Bid.Queries.GetUserPastAuctions
{
    public class GetUserPastAuctionsHandler : IRequestHandler<GetUserPastAuctionsQuery, List<GetUserPastAuctionResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetUserPastAuctionsHandler(IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IBidRepository bidRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task<List<GetUserPastAuctionResponse>> Handle(GetUserPastAuctionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotFoundException("User is not authenticated or user ID is missing.");
            }

            var user = await _userRepository.FindAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var bids = await _bidRepository.FindBidsByUserIdAsync(userId, cancellationToken);
            if (bids == null || !bids.Any())
            {
                throw new NotFoundException("No bids found for the user.");
            }

            var responseList = bids.Select(bid => bid.Koi).Select(koi => new GetUserPastAuctionResponse
            {
                Id = koi.ID,
                Name = koi.Name,
                Sex = koi.Sex,
                Size = koi.Size,
                Age = koi.Age,
                Location = koi.Location,
                Variety = koi.Variety,
                ReservePrice = koi.InitialPrice,
                HighestPrice = koi.Bids.Any() ? koi.Bids.Max(bid => bid.BidAmount) : koi.InitialPrice,
                Description = koi.Description,
                ImageUrl = koi.ImageUrl,
                AuctionRequestStatus = koi.AuctionRequestStatus,
                AuctionStatus = koi.AuctionStatus,
                StartTime = koi.StartTime,
                EndTime = koi.EndTime,
                AllowAutoBid = koi.AllowAutoBid,
                AuctionMethodName = koi.AuctionMethod != null ? koi.AuctionMethod.Name : null,
                BreederName = koi.Breeder != null ? koi.Breeder.UserName : null,
                Contact = koi.Breeder != null ? koi.Breeder.PhoneNumber : null,
                Bidders = koi.Bids.Select(bid => new BidderDto
                {
                    BidderName = bid.Bidder.UserName,
                    BidAmount = bid.BidAmount,
                    BidTime = bid.BidTime.GetValueOrDefault()
                }).ToList(),
                KoiImages = koi.KoiImages.Select(img => new KoiImageDto
                {
                    Url = img.Url,
                    KoiName = img.Koi.Name,
                }).ToList(),
                DeliveryStatus = koi.AuctionStatus == AuctionStatus.Ended ? DeliveryStatus.OnGoing : DeliveryStatus.Finished
            }).ToList();

            return responseList;
        }
    }
}
