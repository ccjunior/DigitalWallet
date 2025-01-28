using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId);
    }
}
