using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Application.Features.Koi.Queries.GetById
{
    public class GetKoiByIdHandler : IRequestHandler<GetKoiByIdQuery, GetKoiResponse>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly IMapper _mapper;

        public GetKoiByIdHandler(IKoiRepository koiRepository, IMapper mapper)
        {
            _koiRepository = koiRepository;
            _mapper = mapper;
        }

        public async Task<GetKoiResponse> Handle(GetKoiByIdQuery request, CancellationToken cancellationToken)
        {

            var koi = await _koiRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null, cancellationToken);
            if (koi is null)
            {
                throw new NotFoundException("Koi not found");
            }
            return koi.MapToKoiModel(_mapper);
        }
    }
}
