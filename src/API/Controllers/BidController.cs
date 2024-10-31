using Application.Features.Bid.AscendingBidAuction;
using Application.Features.Bid.FixedPriceBid;
using Application.Features.Bid.SealedBidAuction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CUSTOMER")]
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

        /// <summary>
        /// Places a Fixed Price bid on a koi.
        /// </summary>
        /// <param name="command">The command containing the KoiId and BidAmount.</param>
        /// <returns>A response indicating the result of the bid placement.</returns>
        /// <response code="200">Returns the result of the bid placement.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost("fixed-price")]
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
        public async Task<IActionResult> PlaceAscendingBidAuction([FromBody] AscendingBidAuctionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        ///// <summary>
        ///// Places a Descending bid on a koi.
        ///// </summary>
        ///// <param name="command">The command containing the KoiId and BidAmount.</param>
        ///// <returns>A response indicating the result of the bid placement.</returns>
        ///// <response code="200">Returns the result of the bid placement.</response>
        ///// <response code="400">If the command is invalid.</response>
        //[HttpPost("descending-bid")]
        //public async Task<IActionResult> PlaceDescendingBidAuction([FromBody] PlaceDescendingBidAuctionCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid command.");
        //    }
        //
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}
    }
}
