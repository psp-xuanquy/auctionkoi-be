using Application.Features.Bid.Queries;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.Bid.Queries.GetBidsForKoi
{
    public class GetBidsForKoiHandler : IRequestHandler<GetBidsForKoiQuery, List<GetBidResponse>>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;

        public GetBidsForKoiHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBidResponse>> Handle(GetBidsForKoiQuery request, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetBidsByKoiIdAsync(b => b.KoiID == request.KoiId && b.DeletedBy == null, cancellationToken);
            if (bids == null || !bids.Any())
            {
                throw new NotFoundException("No Bids found for the specified Koi.");
            }
            return bids.MapToBidModelList(_mapper);
        }
    }
}
