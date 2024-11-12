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

namespace Application.Features.Bid.Queries.GetUserPastAuctions;
public class GetUserPastAuctionsHandler : IRequestHandler<GetUserPastAuctionsQuery, List<GetUserPastAuctionResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBidRepository _bidRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserPastAuctionsHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IBidRepository bidRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _bidRepository = bidRepository;
        _mapper = mapper;
    }

    public async Task<List<GetUserPastAuctionResponse>> Handle(GetUserPastAuctionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new NotFoundException("User is not authenticated or user ID is missing.");
        }

        var user = await _userRepository.FindAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var bids = await _bidRepository.FindBidsByUserIdAsync(userId, cancellationToken);
        if (bids == null || !bids.Any())
        {
            throw new NotFoundException("No bids found for the user.");
        }

        return _mapper.Map<List<GetUserPastAuctionResponse>>(bids); ;
    }
}
