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
public class GetAllKoiHandler : IRequestHandler<GetAllKoiQuery, List<GetAllKoiResponse>>
{
    private readonly IKoiRepository _KoiRepository;
    private readonly IMapper _mapper;

    public GetAllKoiHandler(IKoiRepository KoiRepository, IMapper mapper)
    {
        _KoiRepository = KoiRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllKoiResponse>> Handle(GetAllKoiQuery request, CancellationToken cancellationToken)
    {

        var list = await _KoiRepository.FindAllAsync(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllKoiResponse>>(list);
    }
}
