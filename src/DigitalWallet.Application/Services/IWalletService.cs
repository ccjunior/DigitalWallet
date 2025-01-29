using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Domain.Dtos.Response;

namespace DigitalWallet.Application.Services
{
    public interface IWalletService
    {
        Task<ServiceResult<UserBalanceResponse>> GetWalletByUserIdAsync(Guid userId);
    }
}
