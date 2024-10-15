using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.User.Queries;
using MediatR;

namespace Application.Features.Manager.Queries.GetAllKoiFarmBreeder;
public class GetAllKoiFarmBreederQuery : IRequest<List<GetAllKoiFarmBreederResponse>>, IQuery
{
    public GetAllKoiFarmBreederQuery()
    {

    }
}
