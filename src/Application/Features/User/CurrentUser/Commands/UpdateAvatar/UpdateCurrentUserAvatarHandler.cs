using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.User.CurrentUser.Commands.UpdateAvatar;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.CurrentUser.Commands.UpdateInfo;
public class UpdateCurrentUserAvatarHandler : IRequestHandler<UpdateCurrentUserAvatarRequest, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateCurrentUserAvatarHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserResponse> Handle(UpdateCurrentUserAvatarRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        user.UrlAvatar = request.Command.UrlAvatar;

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
