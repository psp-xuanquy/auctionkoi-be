using Application.Features.Auction.Queries;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.Auction.Queries.GetById
{
    public class GetAuctionByIdHandler : IRequestHandler<GetAuctionByIdQuery, GetAuctionResponse>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;

        public GetAuctionByIdHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<GetAuctionResponse> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {

            //var auction = await _auctionRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null, cancellationToken);
            var auction = await _auctionRepository.GetAuctionWithDetailsAsync(x => x.ID == request.Id && x.DeletedBy == null, cancellationToken);
            if (auction is null)
            {
                throw new NotFoundException("Auction not found");
            }
            return auction.MapToAuctionModel(_mapper);
        }
    }
}
