using Application.Features.Koi.Commands.Update;
using Application.Features.Koi;
using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using Application.Features.Notification.Commands.Create;
using Application.Features.Notification.Queries.Get;
using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Notification.Commands.Update;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly ISender _mediator;

    public NotificationController(ISender mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
    public async Task<ActionResult<List<GetNotificationResponse>>> GetAllKoiFarm(
           CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetNotificationQuery(), cancellationToken);
        return Ok(new JsonResponse<List<GetNotificationResponse>>("Get All Notifications successfully.", result));
    }

    [HttpPost]
    [Authorize(Roles = "MANAGER, STAFF")]
    public async Task<ActionResult<string>> SendNotification([FromBody] CreateNotificationCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new JsonResponse<string>("Send Notification successfully", result));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
    public async Task<IActionResult> Update(
           [FromRoute] string id,
           CancellationToken cancellationToken = default)
    { 
        var result = await _mediator.Send(new UpdateNotificationCommand(id: id), cancellationToken);
        return Ok(new JsonResponse<GetNotificationResponse>("Mark as Read Notification", result));
    }
}
