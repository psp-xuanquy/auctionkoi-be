using Application.Common.Exceptions;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.Koi.Queries.Search
{
    public class SearchKoiHandler : IRequestHandler<SearchKoiQuery, List<GetKoiResponse>>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IMapper _mapper;

        public SearchKoiHandler(IKoiRepository koiRepository, IMapper mapper)
        {
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<List<GetKoiResponse>> Handle(SearchKoiQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Keyword))
            {
                throw new BadRequestException("Keyword cannot be empty");
            }

            var keyword = request.Keyword.ToLower();

            var list = await _koiRepository.FindAllAsync(
                koi =>
                    (koi.Name != null && koi.Name.ToLower().Contains(keyword)) ||
                    (koi.Description != null && koi.Description.ToLower().Contains(keyword)) ||
                    (koi.BreederID != null && koi.BreederID.ToLower().Contains(keyword)) ||
                    koi.Rating.ToString().Contains(keyword) ||
                    koi.Age.ToString().Contains(keyword) ||
                    koi.InitialPrice.ToString().Contains(keyword),
                cancellationToken
            );

            if (list == null || !list.Any())
            {
                throw new NotFoundException("No Koi found matching the keyword");
            }

            return list.MapToKoiModelList(_mapper);
        }
    }
}
