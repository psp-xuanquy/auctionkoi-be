using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Queries.GetAll;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions;
public class GetAllActiveAuctionsHandler : IRequestHandler<GetAllActiveAuctionsQuery, List<KoiResponse>>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;

    public GetAllActiveAuctionsHandler(IKoiRepository koiRepository, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _mapper = mapper;
    }

    public async Task<List<KoiResponse>> Handle(GetAllActiveAuctionsQuery request, CancellationToken cancellationToken)
    {

        var list = await _koiRepository.GetActiveAuctions(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<KoiResponse>>(list);
    }
}
