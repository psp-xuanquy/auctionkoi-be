using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using AutoMapper;
using Domain.IRepositories;
using KN_EXE201.Application.Features.Category.Queries.GetById;
using KoiAuction.Domain.Common.Exceptions;
using MediatR;

namespace Application.Features.KoiBreeder.Queries.GetKoiFarmBreederById
{
    public class GetKoiFarmBreederByIdHandler : IRequestHandler<GetKoiFarmBreederByIdQuery, GetAllKoiFarmBreederResponse>
    {
        private readonly IKoiBreederRepository _koiBreederRepository;
        private readonly IMapper _mapper;

        public GetKoiFarmBreederByIdHandler(IKoiBreederRepository koiBreederRepository, IMapper mapper)
        {
            _koiBreederRepository = koiBreederRepository;
            _mapper = mapper;
        }

        public async Task<GetAllKoiFarmBreederResponse> Handle(GetKoiFarmBreederByIdQuery request, CancellationToken cancellationToken)
        {
            var koiBreeder = await _koiBreederRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null && x.DeletedTime == null, cancellationToken);
            if (koiBreeder is null)
            {
                throw new NotFoundException("KoiBreeder not found");
            }
            return _mapper.Map<GetAllKoiFarmBreederResponse>(koiBreeder);
        }
    }
}
