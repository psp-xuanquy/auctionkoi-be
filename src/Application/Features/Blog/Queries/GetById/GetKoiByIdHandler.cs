using Application.Features.Blog.Queries.GetAll;
using Application.Features.Koi.Queries.GetById;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Blog.Queries.GetById;
public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, GetAllBlogResponse>
{
    private readonly IBlogRepository _BlogRepository;
    private readonly IMapper _mapper;

    public GetBlogByIdHandler(IBlogRepository BlogRepository, IMapper mapper)
    {
        _BlogRepository = BlogRepository;
        _mapper = mapper;
    }

    public async Task<GetAllBlogResponse> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _BlogRepository.FindAsync(x => x.ID == request.Id && x.DeletedBy == null && x.DeletedTime == null, cancellationToken);
        if (blog is null)
        {
            throw new NotFoundException("Auction not found");
        }

        return _mapper.Map<GetAllBlogResponse>(blog);
    }
}
