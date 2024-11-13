using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Queries.GetAll;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions
{
    public class GetAllActiveAuctionsHandler : IRequestHandler<GetAllActiveAuctionsQuery, List<KoiResponse>>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IMapper _mapper;

        public GetAllActiveAuctionsHandler(IKoiRepository koiRepository, IMapper mapper)
        {
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<List<KoiResponse>> Handle(GetAllActiveAuctionsQuery request, CancellationToken cancellationToken)
        {
            var list = await _koiRepository.GetActiveAuctions(cancellationToken);
            if (list == null || !list.Any())
            {
                throw new NotFoundException("No active auctions found.");
            }

            var responseList = list.Select(koi => new KoiResponse
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
                }).ToList()
            }).ToList();

            return responseList;
        }
    }
}
