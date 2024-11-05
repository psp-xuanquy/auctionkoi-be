using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Notification.Commands.Create;
public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationRepository _notificationRepository;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;


    public CreateNotificationHandler(IUserRepository userRepository, ICurrentUserService currentUserService, INotificationRepository notificationRepository, UserManager<UserEntity> userManager, IMapper mapper)
    {
        _currentUserService = currentUserService;
        _mapper = mapper;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _userManager = userManager;
    }

    public async Task<string> Handle(CreateNotificationCommand request, CancellationToken cancellationToken) {
        var login = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (login == null)
        {
            throw new NotFoundException("Please login again");
        }

        var users = await _userRepository.FindAllAsync();

        foreach (var user in users) {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("CUSTOMER") || roles.Contains("KOIBREEDER"))
            {
                var notification = _mapper.Map<NotificationEntity>(request);
                notification.UserID = user.Id;
                notification.MarkAsRead = false;
                notification.CreatedTime = DateTime.UtcNow;
                notification.CreatedBy = login.UserName;
                _notificationRepository.Add(notification);
            }
        }

        var result = await _notificationRepository.UnitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new Exception("Failed to create Notification!");
        }

        return request.Message;
    }

}
