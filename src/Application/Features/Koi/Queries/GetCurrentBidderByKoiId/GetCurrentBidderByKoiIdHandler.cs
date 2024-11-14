using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Common.Exceptions;
using MediatR;

namespace Application.Features.Koi.Queries.GetCurrentBidderByKoiId
{
    public class GetCurrentBidderByKoiIdHandler : IRequestHandler<GetCurrentBidderByKoiIdQuery, BidderDto>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetCurrentBidderByKoiIdHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IKoiRepository koiRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<BidderDto> Handle(GetCurrentBidderByKoiIdQuery request, CancellationToken cancellationToken)
        {
            var bidder = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (bidder == null)
            {
                throw new NotFoundException("Please login again");
            }

            var koi = await _koiRepository.FindAsync(x => x.ID == request.KoiId && x.DeletedBy == null && x.DeletedTime == null, cancellationToken);
            if (koi is null)
            {
                throw new NotFoundException("Auction not found");
            }

            var highestBid = koi.Bids.OrderByDescending(b => b.BidAmount).FirstOrDefault();
            if (highestBid == null)
            {
                throw new InvalidOperationException("No bids found for this Koi auction.");
            }

            var bidderDto = _mapper.Map<BidderDto>(highestBid);
            bidderDto.BidderName = highestBid.Bidder.UserName;
            bidderDto.BidAmount = highestBid.BidAmount;
            bidderDto.BidTime = highestBid.BidTime;

            return bidderDto;
        }
    }
}
