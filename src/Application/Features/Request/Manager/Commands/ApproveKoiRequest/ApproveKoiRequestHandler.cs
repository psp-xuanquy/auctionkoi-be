using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Commands.ApproveRoleRequest;
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

namespace Application.Features.Request.Manager.Commands.ApproveKoiRequest;
public class ApproveKoiRequestHandler : IRequestHandler<ApproveKoiRequestCommand, string>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IKoiRepository _koiRepository;
    private readonly INotificationRepository _notificationRepository;

    public ApproveKoiRequestHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IKoiRepository koiRepository, INotificationRepository notificationRepository)
    {    
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiRepository = koiRepository;  
        _notificationRepository = notificationRepository;
    }

    public async Task<string> Handle(ApproveKoiRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var koi = await _koiRepository.FindAsync(x => x.ID == request.KoiID && x.AuctionRequestStatus == AuctionRequestStatus.Pending);
        if (koi == null)
        {
            throw new NotFoundException("Request not found or has already been approved/denied.");
        }

        koi.AuctionRequestStatus = AuctionRequestStatus.Approved;
        koi.RequestResponse = "Your KoiAuction request has been approved and is now listed on the official auction page.";
        _koiRepository.Update(koi);
        await _koiRepository.UnitOfWork.SaveChangesAsync();

        var notification = new NotificationEntity
        {
            UserID = koi.BreederID,
            Message = "Your Koi request has been approved. You can now track it on the main page.",
            MarkAsRead = false,
            CreatedTime = DateTime.UtcNow,
            CreatedBy = "System"
        };
        _notificationRepository.Add(notification);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();

        return $"You have successfully approved the Koi Auction name {koi.Name}.";
    }
}
