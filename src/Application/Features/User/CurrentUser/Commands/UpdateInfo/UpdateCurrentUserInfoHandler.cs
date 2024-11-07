using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.CurrentUser.Commands.UpdateInfo;
public class UpdateCurrentUserInfoHandler : IRequestHandler<UpdateCurrentUserInfoCommand, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateCurrentUserInfoHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserResponse> Handle(UpdateCurrentUserInfoCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        var user = await _userRepository.FindAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.FullName = request.FullName;
        user.Address = request.Address;
        user.PhoneNumber = request.PhoneNumber;
        //user.UrlAvatar = request.UrlAvatar;

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
