using MediatR;
using KoiAuction.Domain.IRepositories;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;

namespace KoiAuction.Application.User.Queries.GetAll
{
    public class GetAllUserAccountHandler : IRequestHandler<GetAllUserAccountQuery, List<GetUserAccountResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserAccountHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<GetUserAccountResponse>> Handle(GetAllUserAccountQuery request, CancellationToken cancellationToken)
        {

            var list = await _userRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
            if (list is null)
            {
                throw new NotFoundException("Empty list");
            }
            return _mapper.Map<List<GetUserAccountResponse>>(list);
        }
    }
}
