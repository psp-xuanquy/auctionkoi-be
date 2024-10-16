using KoiAuction.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EnumController : ControllerBase
{
    private readonly ISender _mediator;

    public EnumController(ISender meditar)
    {
        _mediator = meditar;
    }

    [HttpGet("variety")]
    public IActionResult GetAllVarieties()
    {
        var varieties = Enum.GetValues(typeof(Variety)).Cast<Variety>().ToList();
        return Ok(varieties);
    }

}
