using Application.Features.AuctionMethod;
using Application.Features.AuctionMethod.Commands.Update;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using Application.Features.Koi;
using Application.Features.Koi.Commands.Create;
using Application.Features.Koi.Commands.Delete;
using Application.Features.Koi.Commands.Update;
using Application.Features.Koi.Queries.Filter;
using Application.Features.Koi.Queries.GetAll;
using Application.Features.Koi.Queries.GetAllActiveAuctions;
using Application.Features.Koi.Queries.GetKoiById;
using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using KN_EXE201.Application.Features.Category.Queries.GetById;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Domain.Enums;
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


        [HttpGet("get-all-kois")]
        [Authorize(Roles = "KOIBREEDER")]
        public async Task<ActionResult<List<KoiResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            try { }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            var result = await _mediator.Send(new GetAllKoiQuery(), cancellationToken);
            return base.Ok(new JsonResponse<List<KoiResponse>>("Get all Kois successfully", result));
        }

        [HttpGet("all-active-auctions")]
        public async Task<ActionResult<JsonResponse<List<KoiResponse>>>> GetAllActiveAuctions(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllActiveAuctionsQuery(), cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return base.Ok(new JsonResponse<List<KoiResponse>>("Get all Active Auctions successfully.", result));
        }

        [HttpGet("get-active-auction-by-koi-id/{id}")]
        public async Task<ActionResult<JsonResponse<KoiResponse>>> GetActiveAuctionByKoiId([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetActiveAuctionByKoiIdQuery(id), cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new JsonResponse<KoiResponse>("Get Active Auction by KoiID successfully.", result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JsonResponse<KoiResponse>>> GetKoiById([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetKoiByIdQuery(id), cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new JsonResponse<KoiResponse>("Get by KoiID successfully.", result));
        }

        [HttpGet("filter")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<JsonResponse<List<KoiResponse>>>> Filter(
            [FromQuery] string? name,
            [FromQuery] double? minLength,
            [FromQuery] double? maxLength,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? breederName,
            [FromQuery] string? auctionMethodName,
            [FromQuery] Sex? sex,
            CancellationToken cancellationToken = default)
        {
            try { }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            var query = new FilterKoiQuery(name, minLength, maxLength, minAge, maxAge, minPrice, maxPrice, breederName, auctionMethodName, sex);
            var result = await _mediator.Send(query, cancellationToken);
            return result != null ? base.Ok(new JsonResponse<List<KoiResponse>>("Filter Koi successfully", result)) : base.NotFound();
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "MANAGER")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<JsonResponse<KoiResponse>>> Create(
            [FromBody] CreateKoiCommand command,
            CancellationToken cancellationToken = default)
        {
            try { }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            var result = await _mediator.Send(command, cancellationToken);
            return base.Ok(new JsonResponse<KoiResponse>("Create Koi successfully", result));
        }

        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<KoiResponse>> Update(
            [FromBody] UpdateKoiCommand command,
            string id,
            CancellationToken cancellationToken = default)
        {
            try { }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            var request = new UpdateKoiRequest(id, command);
            var result = await _mediator.Send(request, cancellationToken);
            return base.Ok(new JsonResponse<KoiResponse>("Update Koi successfully", result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try { }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            var result = await _mediator.Send(new DeleteKoiCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>("Delete Koi successfully", null));
        }
    }
}
