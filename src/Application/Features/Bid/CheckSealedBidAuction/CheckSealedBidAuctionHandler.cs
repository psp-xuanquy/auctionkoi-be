using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.SealedBidAuction
{
    public class CheckSealedBidAuctionHandler : BaseAuctionHandler, IRequestHandler<CheckSealedBidAuctionCommand, bool>
    {
        private readonly IEmailService _emailService;

        public CheckSealedBidAuctionHandler(ICurrentUserService currentUserService, IKoiRepository koiRepository, IBidRepository bidRepository, IUserRepository userRepository, IEmailService emailService)
            : base(koiRepository, bidRepository, userRepository, currentUserService)
        {
            _emailService = emailService;
        }

        public async Task<bool> Handle(CheckSealedBidAuctionCommand request, CancellationToken cancellationToken)
        {
            var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId, cancellationToken);

            if (koi == null)
            {
                return false;
                //throw new NotFoundException($"Koi with ID {request.KoiId} not found.");
            }

            // Check the auction method of the Koi
            if (koi.AuctionMethod.Name != "Sealed Bid Auction")
            {
                return false;
                //throw new BadRequestException($"Koi {koi.Name} is not part of the Sealed Bid Auction.");
            }

            return true;
            //return $"Koi {koi.Name} is part of the Sealed Bid Auction.";
        }
    }
}
