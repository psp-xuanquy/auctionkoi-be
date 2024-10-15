//using Application.Features.AutoBid.Queries;
//using KoiAuction.API.Controllers.ResponseTypes;
//using KoiAuction.Application.Features.AutoBid.Commands.Create;
//using KoiAuction.Application.Features.AutoBid.Commands.Execute;
//using KoiAuction.Application.Features.AutoBid.Queries.GetAll;
//using KoiAuction.Application.Features.AutoBid.Queries.GetAutoBidsForKoi;
//using KoiAuction.Application.Features.AutoBid.Queries.GetById;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KoiAuction.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AutoBidController : ControllerBase
//    {
//        private readonly ISender _mediator;

//        public AutoBidController(ISender mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpGet]
//        public async Task<ActionResult<JsonResponse<List<GetAutoBidResponse>>>> GetAll(CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(new GetAllAutoBidQuery(), cancellationToken);
//            return Ok(new JsonResponse<List<GetAutoBidResponse>>(result));
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<JsonResponse<GetAutoBidResponse>>> GetByID([FromRoute] string id, CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(new GetAutoBidByIdQuery(id), cancellationToken);
//            return result != null ? Ok(new JsonResponse<GetAutoBidResponse>(result)) : NotFound();
//        }

//        [HttpGet("koi/{koiId}")]
//        public async Task<ActionResult<JsonResponse<List<GetAutoBidResponse>>>> GetAutoBidsForKoi([FromRoute] string koiId, CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(new GetAutoBidsForKoiQuery(koiId), cancellationToken);
//            return result != null && result.Any() ? Ok(new JsonResponse<List<GetAutoBidResponse>>(result)) : NotFound();
//        }

//        [HttpPost("create")]
//        [Authorize(Roles = "Manager")]
//        public async Task<ActionResult<JsonResponse<string>>> Create([FromBody] CreateAutoBidCommand command, CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(command, cancellationToken);
//            return Ok(new JsonResponse<string>(result));
//        }

//        [HttpPost("execute")]
//        [Authorize(Roles = "Manager")] 
//        public async Task<ActionResult<JsonResponse<string>>> ExecuteAutoBids(
//            [FromBody] ExecuteAutoBidCommand command,
//            CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(command, cancellationToken);
//            return Ok(new JsonResponse<string>(result));
//        }

//        /*
//        [HttpPut]
//        [Authorize(Roles = "Manager")]
//        public async Task<ActionResult<JsonResponse<string>>> Update([FromBody] UpdateAutoBidCommand command, CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(command, cancellationToken);
//            return Ok(new JsonResponse<string>(result));
//        }
//        */

//        /*
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Manager")]
//        public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
//        {
//            var result = await _mediator.Send(new DeleteAutoBidCommand(id), cancellationToken);
//            return Ok(new JsonResponse<string>(result));
//        }
//        */
//    }
//}
