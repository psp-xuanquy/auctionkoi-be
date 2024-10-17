using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuctionMethod.Queries.GetAll;
public class GetAllAuctionMethodQuery : IRequest<List<GetAllAuctionMethodResponse>>, IQuery
{
    public GetAllAuctionMethodQuery()
    {

    }
}
