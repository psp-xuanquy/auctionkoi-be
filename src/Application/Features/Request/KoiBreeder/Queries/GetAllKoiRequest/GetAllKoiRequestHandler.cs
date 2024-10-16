using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using AutoMapper;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
public class GetAllKoiRequestHandler : IRequestHandler<GetAllKoiRequestQuery, List<GetAllKoiRequestResponse>>
{
    private readonly IKoiRepository _koiRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetAllKoiRequestHandler(IKoiRepository koiRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<GetAllKoiRequestResponse>> Handle(GetAllKoiRequestQuery request, CancellationToken cancellationToken)
    {
        var list = await _koiRepository.FindAllAsync(x => x.BreederID == _currentUserService.UserId, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllKoiRequestResponse>>(list);
    }
}
