using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Request.Manager.Commands.ApproveKoiRequest;
public class ApproveKoiRequestCommand : IRequest<string>
{
    public string KoiID { get; set; }
}
