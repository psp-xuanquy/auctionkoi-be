using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Request.Manager.Commands.DenyRoleRequest;
public class DenyRoleRequestCommand : IRequest<string>
{
    public string KoiBreederID { get; set; }
    public string RequestResponse { get; set; }
}
