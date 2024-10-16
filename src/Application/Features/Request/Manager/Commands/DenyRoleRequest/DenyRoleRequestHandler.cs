using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Request.Manager.Commands.DenyRoleRequest;
public class DenyRoleRequestHandler : IRequestHandler<DenyRoleRequestCommand, string>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IKoiBreederRepository _koiBreederRepository;

    public DenyRoleRequestHandler(UserManager<UserEntity> userManager, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiBreederRepository koiBreederRepository)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiBreederRepository = koiBreederRepository;
    }

    public async Task<string> Handle(DenyRoleRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var koibreeder = await _koiBreederRepository.FindAsync(x => x.ID == request.KoiBreederID && x.RoleRequestStatus == RoleRequestStatus.Pending);
        if (koibreeder == null)
        {
            throw new NotFoundException("Request not found or has already been approved/denied.");
        }

        koibreeder.RoleRequestStatus = RoleRequestStatus.Denied;
        koibreeder.RequestResponse = request.RequestResponse;

        _koiBreederRepository.Update(koibreeder);
        await _koiBreederRepository.UnitOfWork.SaveChangesAsync();

        return $"{koibreeder.KoiFarmName} request has been denied";

    }
}
