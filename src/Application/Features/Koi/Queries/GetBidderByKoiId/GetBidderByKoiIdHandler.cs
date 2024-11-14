using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Queries.GetAll;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using Application.Features.Koi.Queries.GetAllActiveAuctions;
using Application.Features.Koi.Queries.GetBidderByKoiId;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetBidderByKoiId
{
    public class GetBidderByKoiIdHandler : IRequestHandler<GetBidderByKoiIdQuery, List<GetBidderByKoiIdResponse>>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IMapper _mapper;

        public GetBidderByKoiIdHandler(IKoiRepository koiRepository, IMapper mapper)
        {
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBidderByKoiIdResponse>> Handle(GetBidderByKoiIdQuery request, CancellationToken cancellationToken)
        {
            var koi = await _koiRepository.FindAsync(x => x.ID == request.KoiId
                && x.DeletedBy == null
                && x.DeletedTime == null
                , cancellationToken);

            if (koi is null)
            {
                throw new NotFoundException("Auction not found");
            }

            var bidders = koi.Bids.Select(bid => new BidderDto
            {
                BidderName = bid.Bidder.UserName,
                BidAmount = bid.BidAmount,
                BidTime = bid.BidTime.GetValueOrDefault()
            }).ToList();

            var response = new GetBidderByKoiIdResponse
            {
                Bidders = bidders
            };

            return new List<GetBidderByKoiIdResponse> { response };
        }
    }
}
