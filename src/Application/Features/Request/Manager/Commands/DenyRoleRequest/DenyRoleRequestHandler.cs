using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Repositories;
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
    private readonly INotificationRepository _notificationRepository;

    public DenyRoleRequestHandler(UserManager<UserEntity> userManager, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiBreederRepository koiBreederRepository, INotificationRepository notificationRepository)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiBreederRepository = koiBreederRepository;
        _notificationRepository = notificationRepository;
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

        var notification = new NotificationEntity
        {
            UserID = koibreeder.UserId,
            Message = $"You have failed to register as a KoiBreeder for the following reason: {request.RequestResponse}. Please review your information and resubmit request next time.",
            MarkAsRead = false,
            CreatedTime = DateTime.UtcNow,
            CreatedBy = "System"
        };
        _notificationRepository.Add(notification);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();


        return $"You have denied the request from account {koibreeder.User.UserName}.";

    }
}
