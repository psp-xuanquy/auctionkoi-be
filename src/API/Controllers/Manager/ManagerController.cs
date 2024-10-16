using MediatR;
using Microsoft.AspNetCore.Mvc;
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

       
        
        
    }
}
