using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Update;
public class UpdateAuctionMethodRequest : IRequest<AuctionMethodResponse>
{
    public string Id { get; set; }
    public UpdateAuctionMethodCommand Command { get; set; }

    public UpdateAuctionMethodRequest(string id, UpdateAuctionMethodCommand command)
    {
        Id = id;
        Command = command;
    }
}
