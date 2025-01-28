using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Domain.Dtos.Response;

namespace DigitalWallet.Application.Services
{
    public interface IWalletService
    {
        Task<ServiceResult<UserBalanceResponse>> GetBalanceAsync(Guid userId);
        Task AddFundsAsync(Guid userId, decimal amount);
        Task TransferFundsAsync(Guid fromUserId, Guid toUserId, decimal amount);
    }
}
