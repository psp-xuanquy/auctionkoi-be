using Application.Features.Auction.Queries;
using Application.Features.KoiAuctionRequest;
using Application.Features.KoiAuctionRequest.Command;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Features.Auction.Commands.Create;
using KoiAuction.Application.Features.Auction.Queries;
using KoiAuction.Application.Features.Auction.Queries.GetAll;
using KoiAuction.Application.Features.Auction.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuctionController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAuctionResponse>>> GetAll(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllAuctionQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAuctionResponse>>(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JsonResponse<GetAuctionResponse>>> GetByID(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAuctionByIdQuery(id: id), cancellationToken);
            return result != null ? Ok(new JsonResponse<GetAuctionResponse>(result)) : NotFound();
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateAuctionCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPost("request-auction")]
        [Authorize(Roles = "Breeder, Staff")]
        public async Task<IActionResult> RequestAuction(
           [FromBody] KoiAuctionRequestCommand command,
           CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        //[HttpPut]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<string>> Update([FromBody] UpdateAuctionCommand command, CancellationToken cancellationToken = default)
        //{
        //    var result = await _mediator.Send(command, cancellationToken);
        //    return Ok(new JsonResponse<string>(result));
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        //{
        //    var result = await _mediator.Send(new DeleteAuctionCommand(id: id), cancellationToken);
        //    return Ok(new JsonResponse<string>(result));
        //}
    }
}
