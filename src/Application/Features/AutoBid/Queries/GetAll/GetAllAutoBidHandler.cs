//using AutoMapper;
//using KoiAuction.Application.Common.Interfaces;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Application.Features.AutoBid.Queries;

//namespace KoiAuction.Application.Features.AutoBid.Queries.GetAll
//{
//    public class GetAllAutoBidHandler : IRequestHandler<GetAllAutoBidQuery, List<GetAutoBidResponse>>
//    {
//        private readonly IAutoBidRepository _autoBidRepository;
//        private readonly IMapper _mapper;

//        public GetAllAutoBidHandler(IAutoBidRepository autoBidRepository, IMapper mapper)
//        {
//            _autoBidRepository = autoBidRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<GetAutoBidResponse>> Handle(GetAllAutoBidQuery request, CancellationToken cancellationToken)
//        {

//            var list = await _autoBidRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
//            if (list is null)
//            {
//                throw new NotFoundException("Empty List");
//            }
//            return list.MapToAutoBidModelList(_mapper);
//        }
//    }
//}
