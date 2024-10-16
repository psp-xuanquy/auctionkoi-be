using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.User.Queries.GetAll;
using KoiAuction.Application.User.Queries;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;

namespace Application.Features.Request.User.Queries.GetRequestCurrentUser;
public class GetRequestCurrentUserHandler : IRequestHandler<GetRequestCurrentUserQuery, List<GetRequestCurrentUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetRequestCurrentUserHandler(IUserRepository userRepository, IKoiBreederRepository koiBreederRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userRepository = userRepository;
        _koiBreederRepository = koiBreederRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<GetRequestCurrentUserResponse>> Handle(GetRequestCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var list = await _koiBreederRepository.FindAllAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetRequestCurrentUserResponse>>(list);
    }
}

