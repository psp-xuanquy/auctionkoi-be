using Application.Features.Bid.AscendingBidAuction;
using Application.Features.Bid.DescendingBidAuction;
using Application.Features.Bid.FixedPriceBid;
using Application.Features.Bid.SealedBidAuction;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Koi;
using System.Security.Claims;
using Application.Features.Bid.GetUserPastAuctions;
using Application.Features.Bid;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;

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
        public async Task<ActionResult<JsonResponse<List<GetUserPastAuctionResponse>>>> GetUserPastAuctions(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
            return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Get User Past Auctions successfully.", result));
        }

        [HttpGet("user/{userId}/past-auctions/manager")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<List<GetUserPastAuctionResponse>>>> GetUserPastAuctionsByManager([FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
            return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Manager get Past Auctions of User successfully.", result));
        }

        [HttpPost("place-bid")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceBid([FromBody] PlaceBidCommand command, CancellationToken cancellationToken)
        {
            if (command == null || string.IsNullOrEmpty(command.KoiId) || command.BidAmount <= 0)
            {
                return BadRequest("Invalid command.");
            }

            var koi = await _mediator.Send(new GetActiveAuctionByKoiIdQuery(command.KoiId), cancellationToken);
            if (koi == null)
            {
                return NotFound("Koi not found.");
            }

            switch (koi.AuctionMethodName)
            {
                case "Fixed Price Sale":
                    var fixedPriceCommand = new PlaceFixedPriceBidCommand(command.KoiId, command.BidAmount);
                    var fixedPriceResult = await _mediator.Send(fixedPriceCommand, cancellationToken);
                    return Ok(new { Message = fixedPriceResult });

                case "Sealed Bid Auction":
                    var sealedBidCommand = new PlaceSealedBidAuctionCommand(command.KoiId, command.BidAmount);
                    var sealedBidResult = await _mediator.Send(sealedBidCommand, cancellationToken);
                    return Ok(new { Message = sealedBidResult });

                case "Ascending Bid Auction":
                    var ascendingBidCommand = new AscendingBidAuctionCommand(command.KoiId, command.BidAmount);
                    var ascendingBidResult = await _mediator.Send(ascendingBidCommand, cancellationToken);
                    return Ok(new { Message = ascendingBidResult });

                case "Descending Bid Auction":
                    var descendingBidCommand = new DescendingBidAuctionCommand(command.KoiId, command.BidAmount);
                    var descendingBidResult = await _mediator.Send(descendingBidCommand, cancellationToken);
                    return Ok(new { Message = descendingBidResult });

                default:
                    return BadRequest("Unsupported auction method for this Koi.");
            }
        }

        ///// <summary>
        ///// Places a Fixed Price bid on a koi.
        ///// </summary>
        ///// <param name="command">The command containing the KoiId and BidAmount.</param>
        ///// <returns>A response indicating the result of the bid placement.</returns>
        ///// <response code="200">Returns the result of the bid placement.</response>
        ///// <response code="400">If the command is invalid.</response>
        //[HttpPost("fixed-price")]
        //[Authorize(Roles = "CUSTOMER")]
        //public async Task<IActionResult> PlaceFixedPriceBid([FromBody] PlaceFixedPriceBidCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid command.");
        //    }

        //    var result = await _mediator.Send(command);
        //    return Ok(new { Message = result });
        //}

        ///// <summary>
        ///// Places a Sealed bid on a koi.
        ///// </summary>
        ///// <param name="command">The command containing the KoiId and BidAmount.</param>
        ///// <returns>A response indicating the result of the bid placement.</returns>
        ///// <response code="200">Returns the result of the bid placement.</response>
        ///// <response code="400">If the command is invalid.</response>
        //[HttpPost("sealed-bid")]
        //[Authorize(Roles = "CUSTOMER")]
        //public async Task<IActionResult> PlaceSealedBidAuction([FromBody] PlaceSealedBidAuctionCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid command.");
        //    }

        //    var result = await _mediator.Send(command);
        //    return Ok(new { Message = result });
        //}

        ///// <summary>
        ///// Places an Ascending bid on a koi.
        ///// </summary>
        ///// <param name="command">The command containing the KoiId and BidAmount.</param>
        ///// <returns>A response indicating the result of the bid placement.</returns>
        ///// <response code="200">Returns the result of the bid placement.</response>
        ///// <response code="400">If the command is invalid.</response>
        //[HttpPost("ascending-bid")]
        //[Authorize(Roles = "CUSTOMER")]
        //public async Task<IActionResult> PlaceAscendingBidAuction([FromBody] AscendingBidAuctionCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid command.");
        //    }

        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// Places a Descending bid on a koi.
        ///// </summary>
        ///// <param name="command">The command containing the KoiId and BidAmount.</param>
        ///// <returns>A response indicating the result of the bid placement.</returns>
        ///// <response code="200">Returns the result of the bid placement.</response>
        ///// <response code="400">If the command is invalid.</response>
        //[HttpPost("descending-bid")]
        //[Authorize(Roles = "CUSTOMER")]
        //public async Task<IActionResult> PlaceDescendingBidAuction([FromBody] DescendingBidAuctionCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid command.");
        //    }

        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}
    }
}
