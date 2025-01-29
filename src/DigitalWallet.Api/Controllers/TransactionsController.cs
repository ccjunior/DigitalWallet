using DigitalWallet.Application.Services;
using DigitalWallet.Domain.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionRequest request)
        {
            var result = await _transactionService.DepositAsync(request.WalletId, request.Amount);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionRequest request)
        {
            var result = await _transactionService.WithdrawAsync(request.WalletId, request.Amount);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var result = await _transactionService.TransferAsync(request.FromWalletId, request.ToWalletId, request.Amount);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
