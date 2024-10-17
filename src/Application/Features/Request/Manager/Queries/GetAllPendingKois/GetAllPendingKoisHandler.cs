using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetAllPendingKois;
public class GetAllPendingKoisHandler : IRequestHandler<GetAllPendingKoisQuery, List<GetAllPendingKoisResponse>>
{
    private readonly IKoiRepository _koiRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetAllPendingKoisHandler(IKoiRepository koiRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _koiRepository = koiRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<GetAllPendingKoisResponse>> Handle(GetAllPendingKoisQuery request, CancellationToken cancellationToken)
    {
        var list = await _koiRepository.FindAllAsync(x => x.AuctionRequestStatus == AuctionRequestStatus.Pending, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllPendingKoisResponse>>(list);
    }
}
