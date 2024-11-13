using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Queries.GetPendingKoiRequestById;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetRoleRequestById;
public class GetRoleRequestByIdQuery : IRequest<GetRoleRequestByIdResponse>, IQuery
{
    public string BreederId { get; set; }
    public GetRoleRequestByIdQuery(string breederId)
    {
        BreederId = breederId;
    }
}
