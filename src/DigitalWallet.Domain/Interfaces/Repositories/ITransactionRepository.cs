using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransfersByWalletIdAsync(Guid walletId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
