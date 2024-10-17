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

namespace Application.Features.AuctionMethod.Commands.Update;
public class UpdateAuctionMethodHandler : IRequestHandler<UpdateAuctionMethodCommand, string>
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

    public async Task<string> Handle(UpdateAuctionMethodCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var auctionMethod = await _auctionMethodRepository.FindAsync(x => x.ID == request.Id);
        if (auctionMethod == null)
        {
            throw new NotFoundException("AuctionMethod not found");
        }

        var update = _mapper.Map(request, auctionMethod);
        if (update != null)
        {
            _auctionMethodRepository.Update(update);
            await _auctionMethodRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "Update AuctionMethod successfully";
        }

        return "Error occurred while updating AutionMethod";
    }
}
