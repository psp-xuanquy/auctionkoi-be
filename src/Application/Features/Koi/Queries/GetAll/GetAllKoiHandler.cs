using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.GetAll;
public class GetAllKoiHandler : IRequestHandler<GetAllKoiQuery, List<KoiResponse>>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;

    public GetAllKoiHandler(IKoiRepository koiRepository, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _mapper = mapper;
    }

    public async Task<List<KoiResponse>> Handle(GetAllKoiQuery request, CancellationToken cancellationToken)
    {

        var list = await _koiRepository.FindAllAsync(cancellationToken)
            ;
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<KoiResponse>>(list);
    }
}
