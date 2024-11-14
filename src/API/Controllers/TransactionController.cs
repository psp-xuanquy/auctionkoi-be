using Application.Features.Blog.Queries.GetAll;
using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using Application.Features.Transaction.Queries.GetAll;
using Application.Features.Wallet.Deposit;
using Application.Features.Wallet.UpdateBalance;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Application.Transaction.Queries.GetAll;
using MailKit.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for handling commands and queries.</param>
        public TransactionController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Transactions")]
        //[Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<List<GetAllTransactionsResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetAllTransactionsQuery(), cancellationToken);
                return Ok(new JsonResponse<List<GetAllTransactionsResponse>>("Get all Transactions successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        [Route("wallet")]
        public async Task<ActionResult<string>> Deposit([FromBody] DepositToWalletCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(new JsonResponse<string>("Create Paymentlink Successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        [HttpGet]
        [Authorize(Roles = "CUSTOMER")]
        [Route("result")]
        public async Task<ActionResult<string>> DepositSuccess(string userID, decimal amount, string status, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new UpdateBalanceCommand(userID: userID, amount: amount, status: status), cancellationToken);
                return Ok(new JsonResponse<string>("Result", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResponse<string>($"An error occurred: {ex.Message}", null));
            }
        }

        

    }
}
