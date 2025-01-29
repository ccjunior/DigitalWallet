using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Domain.Dtos;
using DigitalWallet.Domain.Dtos.Response;

namespace DigitalWallet.Application.Services
{
    public interface ITransactionService
    {
        Task<ServiceResult<TransctionDto>> DepositAsync(Guid walletId, decimal amount);
        Task<ServiceResult<TransctionDto>> WithdrawAsync(Guid walletId, decimal amount);
        Task<ServiceResult<TransctionDto>> TransferAsync(Guid fromWalletId, Guid toWalletId, decimal amount);
        Task<ServiceResult<IEnumerable<TransctionTransferResponse>>> GetTransfersAsync(Guid walletId, DateTime? startDate = null, DateTime? endDate = null);
    }
}

