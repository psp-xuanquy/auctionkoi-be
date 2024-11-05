using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Commands.Update;
using Application.Features.Koi;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Domain.IRepositories;
using Application.Features.Notification.Queries.Get;
using KoiAuction.Domain.Repositories;

namespace Application.Features.Notification.Commands.Update;
public class UpdateNotificationHandler : IRequestHandler<UpdateNotificationCommand, GetNotificationResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    public UpdateNotificationHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, INotificationRepository notificationRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
    }

    public async Task<GetNotificationResponse> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var notification = await _notificationRepository.FindAsync(x => x.ID == request.Id);
        if (notification == null)
        {
            throw new NotFoundException("Notification not found");
        }

        notification.MarkAsRead = true;
        _notificationRepository.Update(notification);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();

        return _mapper.Map<GetNotificationResponse>(notification);
    }
}
