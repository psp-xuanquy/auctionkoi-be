using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.Update; 
public class DeleteUserByManagerHandler : IRequestHandler<DeleteUserByManagerCommand, string>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    public DeleteUserByManagerHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(DeleteUserByManagerCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == request.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        _userRepository.Remove(user);

        var result = await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            return "Successfully deleted User";
        }

        return "Failed to delete User";
    }
}
