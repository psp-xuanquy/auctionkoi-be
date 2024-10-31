using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Commands.Update;
public class UpdateKoiHandler : IRequestHandler<UpdateKoiRequest, KoiResponse>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IKoiRepository _koiRepository;
    public UpdateKoiHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiRepository koiRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiRepository = koiRepository;
    }

    public async Task<KoiResponse> Handle(UpdateKoiRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var koi = await _koiRepository.FindAsync(x => x.ID == request.Id);
        if (koi == null)
        {
            throw new NotFoundException("Koi not found");
        }

        koi.Name = request.Command.Name;
        koi.Sex = request.Command.Sex;
        koi.Size = request.Command.Size;
        koi.Age = request.Command.Age;
        koi.Location = request.Command.Location;
        koi.Variety = request.Command.Variety;
        koi.InitialPrice = request.Command.InitialPrice;
        koi.ImageUrl = request.Command.ImageUrl;
        koi.RequestResponse = request.Command.RequestResponse;
        koi.AuctionRequestStatus = request.Command.AuctionRequestStatus;
        koi.AuctionStatus = request.Command.AuctionStatus;
        koi.StartTime = request.Command.StartTime;
        koi.EndTime = request.Command.EndTime;
        koi.AllowAutoBid = request.Command.AllowAutoBid;
        koi.AuctionMethodID = request.Command.AuctionMethodID;
        koi.BreederID = request.Command.BreederID;
        koi.Description = request.Command.Description;
        koi.LastUpdatedBy = user.UserName;
        koi.LastUpdatedTime = DateTime.Now;

        _koiRepository.Update(koi);
        await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<KoiResponse>(koi); ;

    }
}
