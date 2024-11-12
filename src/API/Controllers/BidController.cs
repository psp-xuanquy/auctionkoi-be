using Application.Features.AuctionMethod.Commands.Delete;
using System.Threading;
using Application.Features.Bid.AscendingBidAuction;
using Application.Features.Bid.DescendingBidAuction;
using Application.Features.Bid.FixedPriceBid;
using Application.Features.Bid.Queries.GetUserPastAuctions;
using Application.Features.Bid.SealedBidAuction;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Koi;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for handling commands.</param>
        public BidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user/past-auctions")]
        [Authorize]
        public async Task<ActionResult<JsonResponse<GetUserPastAuctionResponse>>> GetUserPastAuctions(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
            return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Get User Past Auctions successfully.", result));
        }

        [HttpGet("user/{userId}/past-auctions/manager")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<GetUserPastAuctionResponse>>> GetUserPastAuctionsByManager([FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
            return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Manager get Past Auctions of User successfully.", result));
        }

        /// <summary>
        /// Places a Fixed Price bid on a koi.
        /// </summary>
        /// <param name="command">The command containing the KoiId and BidAmount.</param>
        /// <returns>A response indicating the result of the bid placement.</returns>
        /// <response code="200">Returns the result of the bid placement.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost("fixed-price")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceFixedPriceBid([FromBody] PlaceFixedPriceBidCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        /// <summary>
        /// Places a Sealed bid on a koi.
        /// </summary>
        /// <param name="command">The command containing the KoiId and BidAmount.</param>
        /// <returns>A response indicating the result of the bid placement.</returns>
        /// <response code="200">Returns the result of the bid placement.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost("sealed-bid")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceSealedBidAuction([FromBody] PlaceSealedBidAuctionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        /// <summary>
        /// Places an Ascending bid on a koi.
        /// </summary>
        /// <param name="command">The command containing the KoiId and BidAmount.</param>
        /// <returns>A response indicating the result of the bid placement.</returns>
        /// <response code="200">Returns the result of the bid placement.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost("ascending-bid")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceAscendingBidAuction([FromBody] AscendingBidAuctionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Places a Descending bid on a koi.
        /// </summary>
        /// <param name="command">The command containing the KoiId and BidAmount.</param>
        /// <returns>A response indicating the result of the bid placement.</returns>
        /// <response code="200">Returns the result of the bid placement.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost("descending-bid")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceDescendingBidAuction([FromBody] DescendingBidAuctionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
