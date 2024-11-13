using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetPendingKoiRequestById;
public class GetKoiRequestByIdHandler : IRequestHandler<GetKoiRequestByIdQuery, GetKoiRequestByIdResponse>
{
    private readonly IKoiRepository _koiRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetKoiRequestByIdHandler(IKoiRepository koiRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetKoiRequestByIdResponse> Handle(GetKoiRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(x => x.ID == request.KoiId, cancellationToken);
        if (koi is null)
        {
            throw new NotFoundException("Koi not found!");
        }
        return _mapper.Map<GetKoiRequestByIdResponse>(koi);
    }
}

