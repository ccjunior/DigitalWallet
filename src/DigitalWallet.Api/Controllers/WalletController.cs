using DigitalWallet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionRequest = DigitalWallet.Domain.Dtos.Request.TransactionRequest;

namespace DigitalWallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _walletService.GetBalanceAsync(Guid.Parse(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost("add-balance")]
        public async Task<IActionResult> AddBalance([FromBody] decimal amount)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _walletService.AddFundsAsync(Guid.Parse(userId), amount);
            return Ok();
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionRequest transactionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _walletService.TransferFundsAsync(Guid.Parse(userId), transactionDto.ReceiverUserId, transactionDto.Amount);
            return Ok();
        }

        //[HttpGet("transactions")]
        //public async Task<IActionResult> GetTransactions([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var transactions = await _walletService.GetTransactions(userId, startDate, endDate);
        //    return Ok(transactions);
        //}
    }
}
