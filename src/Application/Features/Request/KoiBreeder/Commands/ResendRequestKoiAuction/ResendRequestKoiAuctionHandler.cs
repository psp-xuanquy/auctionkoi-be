using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using AutoMapper;
using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Request.KoiBreeder.Commands.ResendRequestKoiAuction;
public class ResendRequestKoiAuctionHandler : IRequestHandler<ResendRequestKoiAuctionCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IKoiRepository _koiRepository;
    private readonly IMapper _mapper;

    public ResendRequestKoiAuctionHandler(IUserRepository userRepository,                             
                                      IMapper mapper,
                                      ICurrentUserService currentUserService,
                                      IKoiRepository koiRepository)
    {
        _userRepository = userRepository;    
        _koiRepository = koiRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<string> Handle(ResendRequestKoiAuctionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var existingRequest = await _koiRepository.FindAsync(x => x.ID == request.KoiID && x.BreederID == _currentUserService.UserId && x.AuctionRequestStatus != AuctionRequestStatus.Approved);
        if (existingRequest == null)
        {
            throw new NotFoundException("Your request does not exist or has already been approved.");
        }

        var koiUpdate = _mapper.Map(request, existingRequest);
        if (koiUpdate != null)
        {
            koiUpdate.AuctionRequestStatus = AuctionRequestStatus.Pending;
            _koiRepository.Update(koiUpdate);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "Resend request success. Please wait to be approved.";
        }

        return "Failed to resend request.";
    }
}
