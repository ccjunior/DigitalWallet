using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DigitalWallet.Domain.Interfaces
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task CommitAsync();

        Task RollbackAsync();
      
        Task<IDbContextTransaction> BeginTransactAsync();

        Task CommitTransactAsync(IDbContextTransaction dbContextTransaction);

        Task RollBackTransactionAsync(IDbContextTransaction dbContextTransaction);

        Task OpenConnectAsync();

        Task CloseConnectionAsync();
    }
}
