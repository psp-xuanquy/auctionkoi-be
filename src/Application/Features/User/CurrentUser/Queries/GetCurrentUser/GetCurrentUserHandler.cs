using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.CurrentUser.Queries.GetCurrentUser;
public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper, UserManager<UserEntity> userManager)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = _mapper.Map<GetCurrentUserResponse>(user);
        response.Role = roles.FirstOrDefault(); 

        return response;
    }
}
