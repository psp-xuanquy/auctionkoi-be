using System.Net.Mime;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using KN_EXE201.Application.Features.Category.Queries.GetById;
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
        [Route("get-all-breeders")]
        public async Task<ActionResult<List<GetAllKoiFarmBreederResponse>>> GetAllKoiFarm(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetAllKoiFarmBreederQuery(), cancellationToken);
                return Ok(new JsonResponse<List<GetAllKoiFarmBreederResponse>>("Get all Koi Farm Breeder successfully.", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<GetAllKoiFarmBreederResponse>>> GetKoiFarmBreederById([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetKoiFarmBreederByIdQuery(id), cancellationToken);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(new JsonResponse<GetAllKoiFarmBreederResponse>("Get Koi Farm Breeder successfully.", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

    }
}
