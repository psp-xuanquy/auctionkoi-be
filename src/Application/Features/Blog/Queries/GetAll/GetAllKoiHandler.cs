using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Blog.Queries.GetAll;
public class GetAllBlogHandler : IRequestHandler<GetAllBlogQuery, List<GetAllBlogResponse>>
{
    private readonly IBlogRepository _BlogRepository;
    private readonly IMapper _mapper;

    public GetAllBlogHandler(IBlogRepository BlogRepository, IMapper mapper)
    {
        _BlogRepository = BlogRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllBlogResponse>> Handle(GetAllBlogQuery request, CancellationToken cancellationToken)
    {

        var list = await _BlogRepository.FindAllAsync(cancellationToken);
        if (list is null)
        {
            throw new NotFoundException("Empty list");
        }
        return _mapper.Map<List<GetAllBlogResponse>>(list);
    }
}
