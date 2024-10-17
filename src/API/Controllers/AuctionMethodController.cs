using Application.Features.AuctionMethod.Commands.Create;
using Application.Features.AuctionMethod.Commands.Delete;
using Application.Features.AuctionMethod.Commands.Update;
using Application.Features.AuctionMethod.Queries.GetAll;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionMethodController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuctionMethodController(ISender meditar)
        {
            _mediator = meditar;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllAuctionMethodResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllAuctionMethodQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllAuctionMethodResponse>>(result));
        }

        [HttpPost]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<string>> Create([FromBody] CreateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPut]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<string>> Update([FromBody] UpdateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteAuctionMethodCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
    }
}
