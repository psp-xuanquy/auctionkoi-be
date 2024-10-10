using Application.Features.AutoBid.Queries;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.AutoBid.Queries.GetAutoBidsForKoi
{
    public class GetAutoBidsForKoiHandler : IRequestHandler<GetAutoBidsForKoiQuery, List<GetAutoBidResponse>>
    {
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly IMapper _mapper;

        public GetAutoBidsForKoiHandler(IAutoBidRepository autoBidRepository, IMapper mapper)
        {
            _autoBidRepository = autoBidRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAutoBidResponse>> Handle(GetAutoBidsForKoiQuery request, CancellationToken cancellationToken)
        {
            var autoBids = await _autoBidRepository.GetAutoBidsByKoiIdAsync(b => b.KoiID == request.KoiId && b.DeletedBy == null, cancellationToken);
            if (autoBids == null || !autoBids.Any())
            {
                throw new NotFoundException("No AutoBids found for the specified Koi.");
            }
            return autoBids.MapToAutoBidModelList(_mapper);
        }
    }
}
