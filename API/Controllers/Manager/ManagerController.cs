using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Admin.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly ISender _meditar;

        public ManagerController(ISender meditar)
        {
            _meditar = meditar;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> CreateManagerAccount([FromBody] RegisterManagerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _meditar.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
    }
}
