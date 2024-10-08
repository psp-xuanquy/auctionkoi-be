using Application.Features.Auction.Queries;
using KoiAuction.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Auction.Queries.GetAll
{
    public class GetAllAuctionQuery : IRequest<List<GetAuctionResponse>>, IQuery
    {
        public GetAllAuctionQuery()
        {

        }
    }
}
