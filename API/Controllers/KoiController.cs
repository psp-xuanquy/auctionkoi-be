using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Features.Koi.Commands.Create;
using KoiAuction.Application.Features.Koi.Commands.Delete;
using KoiAuction.Application.Features.Koi.Commands.Update;
using KoiAuction.Application.Features.Koi.Queries;
using KoiAuction.Application.Features.Koi.Queries.GetAll;
using KoiAuction.Application.Features.Koi.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiController : ControllerBase
    {

        private readonly ISender _mediator;

        public KoiController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetKoiResponse>>> GetAll(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoiQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetKoiResponse>>(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JsonResponse<GetKoiResponse>>> GetByID(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetKoiByIdQuery(id: id), cancellationToken);
            return result != null ? Ok(new JsonResponse<GetKoiResponse>(result)) : NotFound();
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateKoiCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<string>> Update([FromBody] UpdateKoiCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteKoiCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
    }
}
