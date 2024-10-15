//using Application.Common.Exceptions;
//using AutoMapper;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace KoiAuction.Application.Features.Koi.Queries.Filter
//{
//    public class FilterKoiHandler : IRequestHandler<FilterKoiQuery, List<GetKoiResponse>>
//    {
//        private readonly IKoiRepository _koiRepository;
//        private readonly IMapper _mapper;

//        public FilterKoiHandler(IKoiRepository koiRepository, IMapper mapper)
//        {
//            _koiRepository = koiRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<GetKoiResponse>> Handle(FilterKoiQuery request, CancellationToken cancellationToken)
//        {
//            var koiList = await _koiRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);

//            var query = koiList.AsQueryable();

//            if (!string.IsNullOrEmpty(request.Name))
//                query = query.Where(k => k.Name.Contains(request.Name));

//            if (request.MinLength.HasValue)
//                query = query.Where(k => k.Length >= request.MinLength.Value);

//            if (request.MaxLength.HasValue)
//                query = query.Where(k => k.Length <= request.MaxLength.Value);

//            if (request.MinAge.HasValue)
//                query = query.Where(k => k.Age >= request.MinAge.Value);

//            if (request.MaxAge.HasValue)
//                query = query.Where(k => k.Age <= request.MaxAge.Value);

//            if (request.MinPrice.HasValue)
//                query = query.Where(k => k.InitialPrice >= request.MinPrice.Value);

//            if (request.MaxPrice.HasValue)
//                query = query.Where(k => k.InitialPrice <= request.MaxPrice.Value);

//            if (!string.IsNullOrEmpty(request.BreederId))
//                query = query.Where(k => k.BreederID == request.BreederId);

//            if (request.Sex.HasValue)
//                query = query.Where(k => k.Sex == request.Sex.Value);

//            if (request.MinRating.HasValue)
//                query = query.Where(k => k.Rating >= request.MinRating.Value);

//            if (request.MaxRating.HasValue)
//                query = query.Where(k => k.Rating <= request.MaxRating.Value);

//            var list = query.ToList();

//            if (list == null || !list.Any())
//            {
//                throw new NotFoundException("No Koi found matching the filters.");
//            }

//            return list.Select(k => _mapper.Map<GetKoiResponse>(k)).ToList();
//        }
//    }
//}
