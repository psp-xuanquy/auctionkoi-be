﻿using Application.Features.AuctionMethod;
using Application.Features.AuctionMethod.Commands.Create;
using Application.Features.AuctionMethod.Commands.Delete;
using Application.Features.AuctionMethod.Commands.Update;
using Application.Features.AuctionMethod.Queries.GetAll;
using Azure;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Retrieves all auction methods.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A list of auction methods.</returns>
        /// <response code="200">Returns the list of auction methods.</response>
        [HttpGet]
        public async Task<ActionResult<JsonResponse<List<GetAllAuctionMethodResponse>>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllAuctionMethodQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllAuctionMethodResponse>>("Successfully retrieved all Auction Methods.", result));
        }

        /// <summary>
        /// Creates a new auction method.
        /// </summary>
        /// <param name="command">The command containing the details of the auction method to create.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The created auction method.</returns>
        /// <response code="200">Returns the created auction method.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<AuctionMethodResponse>>> Create([FromBody] CreateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<AuctionMethodResponse>("Successfully created Auction Method.", result));
        }

        /// <summary>
        /// Updates an existing auction method.
        /// </summary>
        /// <param name="command">The command containing the updated details of the auction method.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <response code="200">Returns a confirmation of the update.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<AuctionMethodResponse>>> Update([FromRoute] string id, [FromBody] UpdateAuctionMethodCommand command, CancellationToken cancellationToken = default)
        {
            var request = new UpdateAuctionMethodRequest(id, command);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new JsonResponse<AuctionMethodResponse>("Successfully updated Auction Method.", result));
        }

        /// <summary>
        /// Deletes an auction method by ID.
        /// </summary>
        /// <param name="id">The ID of the auction method to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <response code="200">Returns a confirmation of the deletion.</response>
        /// <response code="404">If the auction method with the specified ID does not exist.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER, STAFF")]
        public async Task<ActionResult<JsonResponse<string>>> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteAuctionMethodCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>(result, null));
        }
    }
}
