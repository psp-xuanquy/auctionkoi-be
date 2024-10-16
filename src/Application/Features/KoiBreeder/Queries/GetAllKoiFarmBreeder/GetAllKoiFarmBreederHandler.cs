using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using MediatR;
using Domain.IRepositories;
using Domain.Enums;

namespace Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
public class GetAllKoiFarmBreederHandler : IRequestHandler<GetAllKoiFarmBreederQuery, List<GetAllKoiFarmBreederResponse>>
{
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly IMapper _mapper;

    public GetAllKoiFarmBreederHandler(IKoiBreederRepository koiBreederRepository, IMapper mapper)
    {
        _koiBreederRepository = koiBreederRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllKoiFarmBreederResponse>> Handle(GetAllKoiFarmBreederQuery request, CancellationToken cancellationToken)
    {
        var list = await _koiBreederRepository.FindAllAsync(x => x.User.Status == true && x.RoleRequestStatus != RoleRequestStatus.Pending, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllKoiFarmBreederResponse>>(list);
    }
}
