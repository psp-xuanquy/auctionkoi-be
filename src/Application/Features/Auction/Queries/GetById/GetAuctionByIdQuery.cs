using Application.Features.Auction.Queries;
using KoiAuction.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Auction.Queries.GetById
{
    public class GetAuctionByIdQuery : IRequest<GetAuctionResponse>, IQuery
    {
        public string Id;

        public GetAuctionByIdQuery(string id)
        {
            Id = id;
        }
    }
}
