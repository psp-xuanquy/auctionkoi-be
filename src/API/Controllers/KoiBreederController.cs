using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class KoiBreederController : ControllerBase
{
    private readonly ISender _mediator;

    public KoiBreederController(ISender meditar)
    {
        _mediator = meditar;
    }

    [HttpGet]
    [Authorize(Roles = "MANAGER")]
    [Route("koifarms")]
    public async Task<ActionResult<List<GetAllKoiFarmBreederResponse>>> GetAllKoiFarm(
          CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllKoiFarmBreederQuery(), cancellationToken);
        return Ok(new JsonResponse<List<GetAllKoiFarmBreederResponse>>(result));
    }

}
