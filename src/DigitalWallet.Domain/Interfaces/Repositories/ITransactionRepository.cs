using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId);
    }
}
