//using Application.Features.AutoBid.Queries;
//using AutoMapper;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace KoiAuction.Application.Features.AutoBid.Queries.GetById
//{
//    public class GetAutoBidByIdHandler : IRequestHandler<GetAutoBidByIdQuery, GetAutoBidResponse>
//    {
//        private readonly IAutoBidRepository _autoBidRepository;
//        private readonly IMapper _mapper;

//        public GetAutoBidByIdHandler(IAutoBidRepository autoBidRepository, IMapper mapper)
//        {
//            _autoBidRepository = autoBidRepository;
//            _mapper = mapper;
//        }

//        public async Task<GetAutoBidResponse> Handle(GetAutoBidByIdQuery request, CancellationToken cancellationToken)
//        {

//            var AutoBid = await _autoBidRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null, cancellationToken);
//            if (AutoBid is null)
//            {
//                throw new NotFoundException("AutoBid not found");
//            }
//            return AutoBid.MapToAutoBidModel(_mapper);
//        }
//    }
//}
