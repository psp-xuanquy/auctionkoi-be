using Application.Features.Bid.Queries;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Features.Bid.Commands.Create;
using KoiAuction.Application.Features.Bid.Queries;
using KoiAuction.Application.Features.Bid.Queries.GetAll;
using KoiAuction.Application.Features.Bid.Queries.GetBidsForKoi;
using KoiAuction.Application.Features.Bid.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {

        private readonly ISender _mediator;

        public BidController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetBidResponse>>> GetAll(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllBidQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetBidResponse>>(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JsonResponse<GetBidResponse>>> GetByID(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetBidByIdQuery(id: id), cancellationToken);
            return result != null ? Ok(new JsonResponse<GetBidResponse>(result)) : NotFound();
        }

        [HttpGet("koi/{koiId}")]
        public async Task<ActionResult<JsonResponse<List<GetBidResponse>>>> GetBidsForKoi(
            [FromRoute] string koiId,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetBidsForKoiQuery(koiId), cancellationToken);
            return result != null && result.Any() ? Ok(new JsonResponse<List<GetBidResponse>>(result)) : NotFound();
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateBidCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        //[HttpPut]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<string>> Update([FromBody] UpdateBidCommand command, CancellationToken cancellationToken = default)
        //{
        //    var result = await _mediator.Send(command, cancellationToken);
        //    return Ok(new JsonResponse<string>(result));
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        //{
        //    var result = await _mediator.Send(new DeleteBidCommand(id: id), cancellationToken);
        //    return Ok(new JsonResponse<string>(result));
        //}
    }
}
