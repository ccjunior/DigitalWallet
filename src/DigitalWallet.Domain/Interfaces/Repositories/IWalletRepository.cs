using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet> GetByUserIdAsync(Guid userId);
    }
}
