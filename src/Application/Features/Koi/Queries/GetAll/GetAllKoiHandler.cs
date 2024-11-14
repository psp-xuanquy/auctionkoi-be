using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetAll;
public class GetAllKoiHandler : IRequestHandler<GetAllKoiQuery, List<KoiResponse>>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllKoiHandler(IKoiRepository koiRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _koiRepository = koiRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<KoiResponse>> Handle(GetAllKoiQuery request, CancellationToken cancellationToken)
    {

        var list = await _koiRepository.FindAllAsync(x => x.BreederID == _currentUserService.UserId ,cancellationToken);
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
            HighestPrice = koi.Bids.Any() ? koi.Bids.Max(bid => bid.BidAmount) : koi.InitialPrice,
            Description = koi.Description,
            ImageUrl = koi.ImageUrl,
            AuctionRequestStatus = koi.AuctionRequestStatus,
            AuctionStatus = koi.AuctionStatus,
            CurrentDescendedPrice = koi.CurrentDescendedPrice ?? 0,
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
