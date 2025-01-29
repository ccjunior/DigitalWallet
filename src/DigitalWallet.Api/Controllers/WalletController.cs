using DigitalWallet.Application.Services;
using DigitalWallet.Domain.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    }
}
