using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Koi.Queries.GetAll;
using AutoMapper;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Queries.Filter
{
    public class FilterKoiHandler : IRequestHandler<FilterKoiQuery, List<KoiResponse>>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IMapper _mapper;

        public FilterKoiHandler(IKoiRepository koiRepository, IMapper mapper)
        {
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<List<KoiResponse>> Handle(FilterKoiQuery request, CancellationToken cancellationToken)
        {
            var koiList = await _koiRepository.FindAllAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.Name))
            {
                koiList = koiList.Where(k => k.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (request.MinLength.HasValue)
            {
                koiList = koiList.Where(k => k.Size >= request.MinLength.Value).ToList();
            }
            if (request.MaxLength.HasValue)
            {
                koiList = koiList.Where(k => k.Size <= request.MaxLength.Value).ToList();
            }
            if (request.MinAge.HasValue)
            {
                koiList = koiList.Where(k => k.Age >= request.MinAge.Value).ToList();
            }
            if (request.MaxAge.HasValue)
            {
                koiList = koiList.Where(k => k.Age <= request.MaxAge.Value).ToList();
            }
            if (request.MinPrice.HasValue)
            {
                koiList = koiList.Where(k => k.InitialPrice >= request.MinPrice.Value).ToList();
            }
            if (request.MaxPrice.HasValue)
            {
                koiList = koiList.Where(k => k.InitialPrice <= request.MaxPrice.Value).ToList();
            }
            if (!string.IsNullOrEmpty(request.BreederName))
            {
                koiList = koiList.Where(k => k.Breeder.FullName.Contains(request.BreederName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(request.AuctionMethodName))
            {
                koiList = koiList.Where(k => k.AuctionMethod.Name.Contains(request.AuctionMethodName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (request.Sex.HasValue)
            {
                koiList = koiList.Where(k => k.Sex == request.Sex.Value).ToList();
            }

            return _mapper.Map<List<KoiResponse>>(koiList);
        }
    }
}
