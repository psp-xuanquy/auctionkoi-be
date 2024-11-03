using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Blog.Queries.GetAll;
public class GetAllBlogQuery : IRequest<List<GetAllBlogResponse>>, IQuery
{
    public GetAllBlogQuery()
    {

    }
}
