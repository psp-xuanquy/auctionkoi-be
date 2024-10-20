using Application.Features.Auction.FixedPriceBid.End;
using Application.Features.Auction.FixedPriceBid.Start;
using Application.Features.Auction.SealedBidAuction.Start;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuctionController(ISender mediator)
        {
            _mediator = mediator;
        }

    // Đấu giá xác định
        [HttpPost("fixed-price-bid")]
        public async Task<IActionResult> PlaceFixedPriceBid([FromBody] PlaceFixedPriceBidCommand command)
        {
            if (command == null)
                return BadRequest("Invalid command.");

            try
            {
                await _mediator.Send(command);
                return Ok("Bid placed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("end-fixed-price-auction")]
        public async Task<IActionResult> EndFixedPriceAuction([FromBody] EndFixedPriceAuctionCommand command)
        {
            if (command == null)
                return BadRequest("Invalid command.");

            try
            {
                await _mediator.Send(command);
                return Ok("Auction ended successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    // Đấu giá 1 lần
        [HttpPost("sealed-bid-auction")]
        public async Task<IActionResult> PlaceSealedBidAuction([FromBody] PlaceSealedBidAuctionCommand command)
        {
            if (command == null)
                return BadRequest("Invalid command.");

            try
            {
                await _mediator.Send(command);
                return Ok("Sealed bid placed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("end-sealed-bid-auction")]
        public async Task<IActionResult> EndSealedBidAuction([FromBody] EndFixedPriceAuctionCommand command)
        {
            if (command == null)
                return BadRequest("Invalid command.");

            try
            {
                await _mediator.Send(command);
                return Ok("Sealed bid auction ended successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
