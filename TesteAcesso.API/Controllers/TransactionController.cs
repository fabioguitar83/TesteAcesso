using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TesteAcesso.Model.Handlers;

namespace TesteAcesso.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBaseValidator
    {

        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new transfer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-transaction")]
        public async Task<IActionResult> CreateTransaction(TransactionRequest request)
        {
            try
            {
                var response = _mediator.Send(request);
                var result = response.Result;

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ReturnValidations(ex));
            }
        }

        /// <summary>
        /// Return status of a transfer
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-transaction-status/{transactionId}")]
        public async Task<IActionResult> GetTransactionStatus(string transactionId)
        {
            try
            {
                var response = _mediator.Send(new StatusTransactionRequest(transactionId));
                var result = response.Result;

                if (result == null)
                    return NotFound("Transaction not found");

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ReturnValidations(ex));
            }
        }

        /// <summary>
        /// Execute a transfer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("execute-transaction")]
        public async Task<IActionResult> ExecuteTransaction(ExecuteTransactionRequest request)
        {
            try
            {
                var response = _mediator.Send(request);
                var result = response.Result;

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ReturnValidations(ex));
            }
        }

    }
}