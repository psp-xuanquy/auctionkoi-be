﻿using Application.Features.Koi.Commands.Create;
using Application.Features.Koi.Commands.Delete;
using Application.Features.Koi.Commands.Update;
using Application.Features.Koi.Queries.Filter;
using Application.Features.Koi.Queries.GetAll;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="KoiController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for handling commands and queries.</param>
        public KoiController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all koi.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A list of all koi.</returns>
        /// <response code="200">Returns a list of koi successfully retrieved.</response>
        [HttpGet]
        public async Task<ActionResult<List<GetAllKoiResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoiQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoiResponse>>(result));
        }

        /// <summary>
        /// Filters koi based on provided parameters.
        /// </summary>
        /// <param name="name">The name of the koi.</param>
        /// <param name="minLength">The minimum length of the koi.</param>
        /// <param name="maxLength">The maximum length of the koi.</param>
        /// <param name="minAge">The minimum age of the koi.</param>
        /// <param name="maxAge">The maximum age of the koi.</param>
        /// <param name="minPrice">The minimum price of the koi.</param>
        /// <param name="maxPrice">The maximum price of the koi.</param>
        /// <param name="breederName">The name of the breeder.</param>
        /// <param name="auctionMethodName">The name of the auction method.</param>
        /// <param name="sex">The sex of the koi.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A filtered list of koi.</returns>
        /// <response code="200">Returns a list of filtered koi.</response>
        /// <response code="404">If no koi match the filter criteria.</response>
        [HttpGet("filter")]
        public async Task<ActionResult<JsonResponse<List<GetAllKoiResponse>>>> Filter(
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
            var query = new FilterKoiQuery(name, minLength, maxLength, minAge, maxAge, minPrice, maxPrice, breederName, auctionMethodName, sex);
            var result = await _mediator.Send(query, cancellationToken);
            return result != null ? Ok(new JsonResponse<List<GetAllKoiResponse>>(result)) : NotFound();
        }

        /// <summary>
        /// Creates a new koi.
        /// </summary>
        /// <param name="command">The command containing details for creating a new koi.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the creation.</returns>
        /// <response code="200">Returns success message if the koi is created successfully.</response>
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateKoiCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Updates an existing koi.
        /// </summary>
        /// <param name="command">The command containing details for updating the koi.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <response code="200">Returns success message if the koi is updated successfully.</response>
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<string>> Update(
            [FromBody] UpdateKoiCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Deletes a koi by ID.
        /// </summary>
        /// <param name="id">The ID of the koi to delete.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <response code="200">Returns success message if the koi is deleted successfully.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteKoiCommand(id: id), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
    }
}
