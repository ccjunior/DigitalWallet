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
        private readonly IWalletService _walletService;

        public TransactionsController(ITransactionService transactionService, IWalletService walletService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
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

        [HttpGet("transfers")]
        public async Task<IActionResult> GetTransfers([FromQuery]Guid userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var walletId = await GetWalletIdByUserId(userId); 

            if (walletId == Guid.Empty)
                return NotFound("Carteira não encontrada para o usuário.");

            var result = await _transactionService.GetTransfersAsync(walletId, startDate, endDate);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        private async Task<Guid> GetWalletIdByUserId(Guid userId)
        {
            var wallet = await _walletService.GetWalletByUserIdAsync(userId);
            if (wallet.Data == null)
            {
                return Guid.Empty;
            }

            return wallet.Data.WalletId;
        }
    }
}
