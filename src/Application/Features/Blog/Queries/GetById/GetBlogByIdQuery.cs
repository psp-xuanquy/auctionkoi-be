using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Blog.Queries.GetAll;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Koi.Queries.GetById;
public class GetBlogByIdQuery : IRequest<GetAllBlogResponse>, IQuery
{
    public string Id;

    public GetBlogByIdQuery(string id)
    {
        Id = id;
    }
}
