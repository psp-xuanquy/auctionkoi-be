using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiBreederController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KoiBreederController"/> class.
        /// </summary>
        /// <param name="meditar">The mediator instance used for handling queries.</param>
        public KoiBreederController(ISender meditar)
        {
            _mediator = meditar;
        }

        /// <summary>
        /// Gets all koi farms managed by breeders.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A list of all koi farms managed by breeders.</returns>
        /// <response code="200">Returns a list of koi farms successfully retrieved.</response>
        /// <response code="400">If there is an error in the request.</response>
        [HttpGet]
        //[Authorize(Roles = "MANAGER")]
        [Route("koifarms")]
        public async Task<ActionResult<List<GetAllKoiFarmBreederResponse>>> GetAllKoiFarm(
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoiFarmBreederQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoiFarmBreederResponse>>("Get all Koi Farm Breeder successfully.", result));
        }
    }
}
