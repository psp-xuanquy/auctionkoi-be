using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.AuctionMethod.Commands.Delete;
public class DeleteAuctionMethodHandler : IRequestHandler<DeleteAuctionMethodCommand, string>
{
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public DeleteAuctionMethodHandler(IAuctionMethodRepository auctionMethodRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
         _auctionMethodRepository = auctionMethodRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(DeleteAuctionMethodCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId && x.DeletedTime == null, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var auctionMethod = await _auctionMethodRepository.FindAsync(x => x.ID == request.Id && x.DeletedTime == null, cancellationToken);
        if (auctionMethod == null)
        {
            throw new NotFoundException("Auction Method not found.");
        }

        _auctionMethodRepository.Remove(auctionMethod);

        var result = await _auctionMethodRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            return "Successfully deleted Auction Method";
        }

        return "Failed to delete Auction Method";
    }
}
