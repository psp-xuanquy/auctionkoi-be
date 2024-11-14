using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Bid.Commands;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.Queries.CheckUserBidForKoi
{
    public class CheckUserBidForKoiHandler : IRequestHandler<CheckUserBidForKoiCommand, bool>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBidRepository _bidRepository;

        public CheckUserBidForKoiHandler(ICurrentUserService currentUserService, IBidRepository bidRepository)
        {
            _currentUserService = currentUserService;
            _bidRepository = bidRepository;
        }

        public async Task<bool> Handle(CheckUserBidForKoiCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var userHasBid = await _bidRepository.AnyAsync(bid => bid.KoiID == request.KoiId && bid.BidderID == userId, cancellationToken);

            return userHasBid;
        }
    }
}
