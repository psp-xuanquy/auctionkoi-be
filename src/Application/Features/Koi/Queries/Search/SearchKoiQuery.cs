using KoiAuction.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Koi.Queries.Search
{
    public class SearchKoiQuery : IRequest<List<GetKoiResponse>>, IQuery
    {
        public string Keyword { get; set; }

        public SearchKoiQuery(string keyword)
        {
            Keyword = keyword;
        }
    }
}
