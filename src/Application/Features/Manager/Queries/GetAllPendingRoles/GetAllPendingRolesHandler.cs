using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.User.Queries.GetAll;
using KoiAuction.Application.User.Queries;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Domain.IRepositories;
using Domain.Enums;

namespace Application.Features.Manager.Queries.GetAllPendingRoles;
public class GetAllPendingRolesHandler : IRequestHandler<GetAllPendingRolesQuery, List<GetAllPendingRolesResponse>>
{
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly IMapper _mapper;

    public GetAllPendingRolesHandler(IKoiBreederRepository koiBreederRepository, IMapper mapper)
    {
        _koiBreederRepository = koiBreederRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllPendingRolesResponse>> Handle(GetAllPendingRolesQuery request, CancellationToken cancellationToken)
    {
        var list = await _koiBreederRepository.FindAllAsync(x => x.RoleRequestStatus == RoleRequestStatus.Pending, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllPendingRolesResponse>>(list);
    }
}
