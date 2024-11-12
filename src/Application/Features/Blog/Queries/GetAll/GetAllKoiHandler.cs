using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;

namespace Application.Features.Blog.Queries.GetAll;
public class GetAllBlogHandler : IRequestHandler<GetAllBlogQuery, List<GetAllBlogResponse>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public GetAllBlogHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllBlogResponse>> Handle(GetAllBlogQuery request, CancellationToken cancellationToken)
    {

        var list = await _blogRepository.FindAllAsync(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllBlogResponse>>(list);
    }
}
