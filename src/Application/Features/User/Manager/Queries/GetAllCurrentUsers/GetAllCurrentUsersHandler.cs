using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Manager.Queries.GetAllCurrentUsers;
public class GetAllCurrentUsersHandler : IRequestHandler<GetAllCurrentUsersQuery, List<GetAllCurrentUsersResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllCurrentUsersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllCurrentUsersResponse>> Handle(GetAllCurrentUsersQuery request, CancellationToken cancellationToken)
    {

        var list = await _userRepository.FindAllAsync(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllCurrentUsersResponse>>(list);
    }
}
