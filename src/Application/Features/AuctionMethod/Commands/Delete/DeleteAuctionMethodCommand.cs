using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Delete;
public class DeleteAuctionMethodCommand : IRequest<string>
{
    public string Id { get; set; }

    public DeleteAuctionMethodCommand(string id)
    {
        Id = id;
    }
}
