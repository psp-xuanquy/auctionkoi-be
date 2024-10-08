using Application.Features.Bid.Queries;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.Bid.Queries.GetById
{
    public class GetBidByIdHandler : IRequestHandler<GetBidByIdQuery, GetBidResponse>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;

        public GetBidByIdHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task<GetBidResponse> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {

            var bid = await _bidRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null, cancellationToken);
            if (bid is null)
            {
                throw new NotFoundException("Bid not found");
            }
            return bid.MapToBidModel(_mapper);
        }
    }
}
