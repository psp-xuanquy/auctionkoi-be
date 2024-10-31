using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Manager.Queries.GetAllCurrentUsers;
public class GetAllCurrentUsersHandler : IRequestHandler<GetAllCurrentUsersQuery, List<GetAllCurrentUsersResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;

    public GetAllCurrentUsersHandler(IUserRepository userRepository, IMapper mapper, UserManager<UserEntity> userManager)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<List<GetAllCurrentUsersResponse>> Handle(GetAllCurrentUsersQuery request, CancellationToken cancellationToken)
    {
        var list = await _userRepository.FindAllAsync(cancellationToken);
        if (list is null || !list.Any())
        {
            throw new NotFoundException("Empty list");
        }

        var responses = new List<GetAllCurrentUsersResponse>();

        foreach (var user in list)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var response = _mapper.Map<GetAllCurrentUsersResponse>(user);
            response.Role = roles.FirstOrDefault();
            responses.Add(response);
        }

        return responses;
    }
}

