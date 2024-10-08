using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Auction.Queries;
using KoiAuction.Application.Features.Auction;

namespace KoiAuction.Application.Features.Auction.Queries.GetAll
{
    public class GetAllAuctionHandler : IRequestHandler<GetAllAuctionQuery, List<GetAuctionResponse>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMapper _mapper;

        public GetAllAuctionHandler(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAuctionResponse>> Handle(GetAllAuctionQuery request, CancellationToken cancellationToken)
        {

            var list = await _auctionRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
            if (list is null)
            {
                throw new NotFoundException("Empty List");
            }
            return list.MapToAuctionModelList(_mapper);
        }
    }
}
