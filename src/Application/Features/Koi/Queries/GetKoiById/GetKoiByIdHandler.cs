using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetKoiById;
public class GetKoiByIdHandler : IRequestHandler<GetKoiByIdQuery, KoiResponse>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;

    public GetKoiByIdHandler(IKoiRepository koiRepository, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _mapper = mapper;
    }

    public async Task<KoiResponse> Handle(GetKoiByIdQuery request, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null && x.DeletedTime == null, cancellationToken);
        if (koi is null)
        {
            throw new NotFoundException("Auction not found");
        }

        //return _mapper.Map<KoiResponse>(koiBreeder);

        var response = new KoiResponse
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
            CurrentDescendedPrice = koi.CurrentDescendedPrice ?? 0,
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
            }).ToList(),
            KoiImages = koi.KoiImages.Select(img => new KoiImageDto
            {
                Url = img.Url,
                KoiName = img.Koi.Name,
            }).ToList()
        };

        return response;
    }

   
}
