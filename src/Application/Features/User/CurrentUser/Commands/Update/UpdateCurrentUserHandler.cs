using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.Update;
public class UpdateCurrentUserHandler : IRequestHandler<UpdateCurrentUserCommand, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    public UpdateCurrentUserHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        //user.FullName = request.FullName;
        //user.Address = request.Address;
        //user.Gender = request.Gender;
        //user.UrlAvatar = request.UrlAvatar;

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
