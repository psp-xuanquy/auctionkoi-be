using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;

namespace Application.Features.KoiAuctionRequest.Command;
public class KoiAuctionRequestHandler : IRequestHandler<KoiAuctionRequestCommand, string>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IKoiRepository _koiRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public KoiAuctionRequestHandler(IAuctionRepository auctionRepository, IKoiRepository koiRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _auctionRepository = auctionRepository;
        _koiRepository = koiRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(KoiAuctionRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please log in again");
        }

        if (request.IsInspectionRequired)
        {
            bool inspectionPassed = await InspectKoi(request.KoiId, cancellationToken);
            if (!inspectionPassed)
            {
                throw new Exception("Koi inspection failed.");
            }
        }

        var auctionEntity = new AuctionEntity
        {
            AuctionMethodID = request.AuctionMethodId,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow.AddDays(7),
            AllowAutoBid = request.AllowAutoBid,
            Status = Status.Active,
        };

        _auctionRepository.Add(auctionEntity);
        await _auctionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var koi = await _koiRepository.FindAsync(x => x.ID == request.KoiId && x.DeletedTime == null, cancellationToken);
        if (koi != null)
        {
            koi.AuctionID = auctionEntity.ID;
            koi.InitialPrice = request.InitialPrice;

            _koiRepository.Update(koi);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        return "Auction request processed successfully.";
    }

    private async Task<bool> InspectKoi(string koiId, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(x => x.ID == koiId && x.DeletedTime == null, cancellationToken);
        if (koi == null)
        {
            return false;
        }

        return true;
    }
}

