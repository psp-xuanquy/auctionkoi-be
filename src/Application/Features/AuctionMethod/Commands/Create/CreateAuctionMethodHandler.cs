using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.AuctionMethod.Commands.Create;
public class CreateAuctionMethodHandler : IRequestHandler<CreateAuctionMethodCommand, string>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAuctionMethodRepository _auctionMethodRepository;

    public CreateAuctionMethodHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, IAuctionMethodRepository auctionMethodRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _auctionMethodRepository = auctionMethodRepository;
    
    }

    public async Task<string> Handle(CreateAuctionMethodCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var existAuctionMethod = await _auctionMethodRepository.FindAsync(x => x.Name == request.Name, cancellationToken);
        if (existAuctionMethod != null)
        {
            throw new DuplicationException("This AuctionMethod does exist");
        }

        var trip = _mapper.Map<AuctionMethodEntity>(request);
        
        _auctionMethodRepository.Add(trip);
        await _auctionMethodRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return "Create AuctionMethod success";

    }
}
