﻿using Application.Common.Exceptions;
using Application.Features.AuctionMethod;
using Application.Features.AuctionMethod.Commands.Create;
using Application.Features.AuctionMethod.Commands.Delete;
using Application.Features.AuctionMethod.Commands.Update;
using Application.Features.AuctionMethod.Queries.GetAll;
using Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
using Application.Features.Bid.Queries.CheckAscendingBidAuction;
using Application.Features.Bid.Queries.CheckDescendingBidAuction;
using Application.Features.Bid.Queries.CheckSealedBidAuction;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Domain.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionMethodController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionMethodController"/> class.
        /// </summary>
        /// <param name="meditar">The mediator instance used for handling commands and queries.</param>
        public AuctionMethodController(ISender meditar)
        {
            _mediator = meditar;
        }

        [HttpGet]
        public async Task<ActionResult<JsonResponse<List<GetAllAuctionMethodResponse>>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllAuctionMethodQuery(), cancellationToken);
            var sortedResult = result.OrderBy(method => GetAuctionMethodOrder(method.Name)).ToList();
            return Ok(new JsonResponse<List<GetAllAuctionMethodResponse>>("Successfully retrieved all Auction Methods.", sortedResult));
        }

        [HttpGet("check-sealed-bid/{koiId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> CheckSealedBidAuction([FromRoute] string koiId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new CheckSealedBidAuctionCommand(koiId), cancellationToken);

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

        [HttpGet("check-ascending-bid/{koiId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> CheckAscendingBidAuction([FromRoute] string koiId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new CheckAscendingBidAuctionCommand(koiId), cancellationToken);

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

        [HttpGet("check-descending-bid/{koiId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> CheckDescendingBidAuction([FromRoute] string koiId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new CheckDescendingBidAuctionCommand(koiId), cancellationToken);

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

        [HttpPost]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<AuctionMethodResponse>>> Create([FromBody] CreateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<AuctionMethodResponse>("Successfully created Auction Method.", result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<AuctionMethodResponse>>> Update([FromRoute] string id, [FromBody] UpdateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var request = new UpdateAuctionMethodRequest(id, command);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new JsonResponse<AuctionMethodResponse>("Successfully updated Auction Method.", result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteAuctionMethodCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>(result, null));
        }

        [HttpGet("revenue/{year}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<GetRevenueForAllMethodsResponse>>> GetRevenueForEachMethod(int year, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRevenueForEachMethodQuery(year), cancellationToken);
            return Ok(new JsonResponse<GetRevenueForAllMethodsResponse>($"Successfully retrieved Revenue for year {year} for each Auction Method.", result));
        }

        [HttpGet("percentage-users")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<List<GetPercentageUserForEachMethodResponse>>>> GetPercentageUserForEachMethod([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPercentageUserForEachMethodQuery(year, month), cancellationToken);
            return Ok(new JsonResponse<List<GetPercentageUserForEachMethodResponse>>($"Successfully retrieved percentage of users for month {month} of year {year} for Auction Methods.", result));
        }

        private int GetAuctionMethodOrder(string auctionMethodName)
        {
            switch (auctionMethodName)
            {
                case "Fixed Price Sale":
                    return 1;
                case "Sealed Bid Auction":
                    return 2;
                case "Ascending Bid Auction":
                    return 3;
                case "Descending Bid Auction":
                    return 4;
                default:
                    return 5;
            }
        }
    }
}
