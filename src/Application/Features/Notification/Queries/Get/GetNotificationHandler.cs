using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using AutoMapper;
using Domain.IRepositories;
using Infrastructure.Repositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Notification.Queries.Get;
public class GetNotificationHandler : IRequestHandler<GetNotificationQuery, List<GetNotificationResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetNotificationHandler(IUserRepository userRepository, INotificationRepository notificationRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<GetNotificationResponse>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var list = await _notificationRepository.FindAllAsync(x => x.UserID == _currentUserService.UserId, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetNotificationResponse>>(list);
    }
}
