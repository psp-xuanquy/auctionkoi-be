using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.AuctionMethod.Queries.GetAll;
public class GetAllAuctionMethodHandler : IRequestHandler<GetAllAuctionMethodQuery, List<GetAllAuctionMethodResponse>>
{
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    private readonly IMapper _mapper;

    public GetAllAuctionMethodHandler(IAuctionMethodRepository auctionMethodRepository, IMapper mapper)
    {
        _auctionMethodRepository = auctionMethodRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllAuctionMethodResponse>> Handle(GetAllAuctionMethodQuery request, CancellationToken cancellationToken)
    {

        var list = await _auctionMethodRepository.FindAllAsync(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllAuctionMethodResponse>>(list);
    }
}
