using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Commands.ApproveRoleRequest;
using AutoMapper;
using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
public class SendRequestKoiAuctionHandler : IRequestHandler<SendRequestKoiAuctionCommand, string>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;

    public SendRequestKoiAuctionHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IAuctionMethodRepository auctionMethodRepository, IKoiRepository koiRepository, IMapper mapper)
    {     
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _auctionMethodRepository = auctionMethodRepository;  
        _koiRepository = koiRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(SendRequestKoiAuctionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        // Kiểm tra xem AuctionMethodId có hợp lệ không
        var auctionMethod = await _auctionMethodRepository.FindAsync(x => x.ID == request.AuctionMethodID, cancellationToken);
        if (auctionMethod == null)
        {
            throw new NotFoundException("Auction Method not found.");
        }

        var koi = _mapper.Map<KoiEntity>(request);
        koi.CreatedTime = DateTime.UtcNow;
        koi.StartTime = request.StartTime;
        koi.CreatedBy = user.FullName;
        koi.BreederID = user.Id;
        koi.EndTime = (request.StartTime?.AddMinutes(5));
        koi.AuctionStatus = AuctionStatus.NotStarted;
        koi.AuctionRequestStatus = AuctionRequestStatus.Pending;

        _koiRepository.Add(koi);
        var result = await _koiRepository.UnitOfWork.SaveChangesAsync();

        if (result <= 0) 
        {
            throw new Exception("Failed to add Koi to the database.");
        }

        return "Your Koi request has been sent. Please wait to be approved.";

    }
}
