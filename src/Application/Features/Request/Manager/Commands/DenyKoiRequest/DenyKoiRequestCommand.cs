using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Request.Manager.Commands.DenyKoiRequest;
public class DenyKoiRequestCommand : IRequest<string>
{
    public string KoiID { get; set; }
    public string RequestResponse { get; set; }
}

