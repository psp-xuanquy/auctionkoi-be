using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Queries.GetAll;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions;
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
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }

        //return _mapper.Map<List<KoiResponse>>(list);

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
            Description = koi.Description,
            ImageUrl = koi.ImageUrl,
            AuctionRequestStatus = koi.AuctionRequestStatus,
            AuctionStatus = koi.AuctionStatus,
            StartTime = koi.StartTime,
            EndTime = koi.EndTime,
            AllowAutoBid = koi.AllowAutoBid,
            AuctionMethodName = koi.AuctionMethod?.Name,  
            BreederName = koi.Breeder?.UserName,  
            Contact = koi.Breeder?.PhoneNumber, 
            Bidders = koi.Bids.Select(bid => new BidderDto
            {
                BidderName = bid.Bidder.UserName,
                BidAmount = bid.BidAmount,
                BidTime = bid.BidTime.GetValueOrDefault()
            }).ToList()
        }).ToList();

        return responseList;
    }
}
