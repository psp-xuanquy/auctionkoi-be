using Application.Features.Blog.Queries.GetAll;
using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for handling commands and queries.</param>
        public BlogController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all Blog.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the request.</param>
        /// <returns>A list of all Blog.</returns>
        /// <response code="200">Returns a list of Blog successfully retrieved.</response>
        [HttpGet("Blogs")]
        public async Task<ActionResult<List<GetAllBlogResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllBlogQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllBlogResponse>>("Get all Blogs successfully", result));
        }
    }
}
