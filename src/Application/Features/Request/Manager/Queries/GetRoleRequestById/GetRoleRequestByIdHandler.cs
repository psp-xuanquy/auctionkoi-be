using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Queries.GetPendingKoiRequestById;
using AutoMapper;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetRoleRequestById;
public class GetRoleRequestByIdHandler : IRequestHandler<GetRoleRequestByIdQuery, GetRoleRequestByIdResponse>
{
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetRoleRequestByIdHandler(IKoiBreederRepository koiBreederRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _koiBreederRepository = koiBreederRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetRoleRequestByIdResponse> Handle(GetRoleRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _koiBreederRepository.FindAsync(x => x.ID == request.BreederId, cancellationToken);
        if (role is null)
        {
            throw new NotFoundException("KoiBreeder request not found!");
        }
        return _mapper.Map<GetRoleRequestByIdResponse>(role);
    }
}
