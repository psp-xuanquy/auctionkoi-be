﻿using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Features.Bid;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;
using Application.Features.Bid.Queries.GetUserPastAuctions;
using Application.Features.Bid.Commands.SealedBidAuction;
using Application.Features.Bid.Commands.FixedPriceBid;
using Application.Features.Bid.Commands.AscendingBidAuction;
using Application.Features.Bid.Commands.DescendingBidAuction;
using Application.Common.Exceptions;
using Application.Features.Bid.Queries.CheckDescendingBidAuction;
using KoiAuction.Domain.Common.Exceptions;
using Application.Features.Bid.Queries.CheckUserBidForKoi;

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
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
                return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Get User Past Auctions successfully.", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        [HttpGet("user/{userId}/past-auctions/manager")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<List<GetUserPastAuctionResponse>>>> GetUserPastAuctionsByManager([FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetUserPastAuctionsQuery(userId), cancellationToken);
                return Ok(new JsonResponse<List<GetUserPastAuctionResponse>>("Manager get Past Auctions of User successfully.", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }        

        [HttpPost("place-bid")]
        [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
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
                    try
                    {
                        var fixedPriceCommand = new PlaceFixedPriceBidCommand(command.KoiId, command.BidAmount);
                        var fixedPriceResult = await _mediator.Send(fixedPriceCommand, cancellationToken);
                        return Ok(new { Message = fixedPriceResult });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
                    }


                case "Sealed Bid Auction":
                    try
                    {
                        var sealedBidCommand = new PlaceSealedBidAuctionCommand(command.KoiId, command.BidAmount);
                        var sealedBidResult = await _mediator.Send(sealedBidCommand, cancellationToken);
                        return Ok(new { Message = sealedBidResult });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
                    }


                case "Ascending Bid Auction":
                    try
                    {
                        var ascendingBidCommand = new AscendingBidAuctionCommand(command.KoiId, command.BidAmount);
                        var ascendingBidResult = await _mediator.Send(ascendingBidCommand, cancellationToken);
                        return Ok(new { Message = ascendingBidResult });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
                    }

                case "Descending Bid Auction":
                    try
                    {
                        var descendingBidCommand = new DescendingBidAuctionCommand(command.KoiId, command.BidAmount);
                        var descendingBidResult = await _mediator.Send(descendingBidCommand, cancellationToken);
                        return Ok(new { Message = descendingBidResult });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
                    }


                default:
                    return BadRequest("Unsupported auction method for this Koi.");
            }
        }

        [HttpPost("auto-bid")]
        [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
        public async Task<IActionResult> PlaceAutoBid([FromBody] PlaceAutoBidCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(new JsonResponse<string>(result, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        [HttpGet("check-userBid-for-koi/{koiId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> CheckUserBidForKoi([FromRoute] string koiId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new CheckUserBidForKoiCommand(koiId), cancellationToken);

                //return Ok(new JsonResponse<bool>("Koi is part of Sealed Bid Auction.", result));
                return result;
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new JsonResponse<string>($"Koi not found: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
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
