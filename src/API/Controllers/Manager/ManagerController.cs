using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Admin.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Manager.Queries.GetAllPendingRoles;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Manager.Queries.GetAllKoiFarmBreeder;
using Application.Features.Manager.Commands.ApproveRoleRequest;

namespace KoiAuction.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly ISender _mediator;

        public ManagerController(ISender meditar)
        {
            _mediator = meditar;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> CreateManagerAccount([FromBody] RegisterManagerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("pending-role-request")]
        public async Task<ActionResult<List<GetAllPendingRolesResponse>>> GetAllPendingRoles(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingRolesQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingRolesResponse>>(result));
        }

        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("list-koifarm")]
        public async Task<ActionResult<List<GetAllKoiFarmBreederResponse>>> GetAllKoiFarm(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoiFarmBreederQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoiFarmBreederResponse>>(result));
        }

        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("approve/{koiBreederId}")]
        public async Task<ActionResult<JsonResponse<string>>> Approve([FromRoute] string koiBreederId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new ApproveRoleRequestCommand(id: koiBreederId), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
    }
}
