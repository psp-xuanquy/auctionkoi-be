using Application.Features.Blog.Queries.GetAll;
using Application.Features.Transaction.Queries.GetAll;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Transaction.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for handling commands and queries.</param>
        public TransactionController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Transactions")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<List<GetAllTransactionsResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllTransactionsQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllTransactionsResponse>>("Get all Transactions successfully", result));
        }
    }
}
