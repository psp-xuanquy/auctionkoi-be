using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.AuctionMethod.Commands.Update;
public class UpdateAuctionMethodHandler : IRequestHandler<UpdateAuctionMethodRequest, AuctionMethodResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    public UpdateAuctionMethodHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, IAuctionMethodRepository auctionMethodRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _auctionMethodRepository = auctionMethodRepository;
    }

    public async Task<AuctionMethodResponse> Handle(UpdateAuctionMethodRequest request,CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var auctionMethod = await _auctionMethodRepository.FindAsync(x => x.ID == request.Id && x.DeletedTime == null, cancellationToken);
        if (auctionMethod == null)
        {
            throw new NotFoundException("Auction Method not found");
        }

        auctionMethod.Name = request.Command.Name;
        auctionMethod.Description = request.Command.Description;
        auctionMethod.LastUpdatedBy = user.UserName;
        auctionMethod.LastUpdatedTime = DateTime.Now;

        _auctionMethodRepository.Update(auctionMethod);
        await _auctionMethodRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuctionMethodResponse>(auctionMethod);
    }
}
