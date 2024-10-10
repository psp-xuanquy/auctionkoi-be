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
using Application.Features.Bid.Queries;

namespace KoiAuction.Application.Features.Bid.Queries.GetAll
{
    public class GetAllBidHandler : IRequestHandler<GetAllBidQuery, List<GetBidResponse>>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;

        public GetAllBidHandler(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBidResponse>> Handle(GetAllBidQuery request, CancellationToken cancellationToken)
        {

            var list = await _bidRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
            if (list is null)
            {
                throw new NotFoundException("Empty List");
            }
            return list.MapToBidModelList(_mapper);
        }
    }
}
