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

//namespace KoiAuction.Application.Features.Koi.Queries.GetAll
//{
//    public class GetAllKoiHandler : IRequestHandler<GetAllKoiQuery, List<GetKoiResponse>>
//    {
//        private readonly IKoiRepository _koiRepository;
//        private readonly IMapper _mapper;

//        public GetAllKoiHandler(IKoiRepository koiRepository, IMapper mapper)
//        {
//            _koiRepository = koiRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<GetKoiResponse>> Handle(GetAllKoiQuery request, CancellationToken cancellationToken)
//        {

//            var list = await _koiRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
//            if (list is null)
//            {
//                throw new NotFoundException("Empty List");
//            }
//            return list.MapToKoiModelList(_mapper);
//        }
//    }
//}
