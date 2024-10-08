using Application.Features.Bid.Queries;
using KoiAuction.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Bid.Queries.GetById
{
    public class GetBidByIdQuery : IRequest<GetBidResponse>, IQuery
    {
        public string Id;

        public GetBidByIdQuery(string id)
        {
            Id = id;
        }
    }
}
