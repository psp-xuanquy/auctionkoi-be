using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Commands.Update;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.User.Manager.Commands.UpdateUserByManager;
public class UpdateUserByManagerHandler : IRequestHandler<UpdateUserByManagerRequest, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    public UpdateUserByManagerHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(UpdateUserByManagerRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        user.UserName = request.Command.UserName;
        user.Email = request.Command.Email;
        //user.FullName = request.FullName;
        //user.Address = request.Address;
        //user.Gender = request.Gender;
        //user.UrlAvatar = request.UrlAvatar;

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
