using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Koi.Commands.Update;
public class UpdateKoiRequest : IRequest<KoiResponse>
{
    public string Id { get; set; }
    public UpdateKoiCommand Command { get; set; }

    public UpdateKoiRequest(string id, UpdateKoiCommand command)
    {
        Id = id;
        Command = command;
    }
}
