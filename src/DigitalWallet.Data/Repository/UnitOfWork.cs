using DigitalWallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DigitalWallet.Data.Repository
{
    public class UnitOfWork<TContext>(TContext context) 
        : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly DbContext _context = context;

        public async Task CommitAsync()
            => await _context.SaveChangesAsync();

        public async Task RollbackAsync()
            => await _context.DisposeAsync();
        
        public async Task<IDbContextTransaction> BeginTransactAsync()
            => await _context.Database.BeginTransactionAsync();

        public async Task CloseConnectionAsync()
            => await _context.Database.CloseConnectionAsync();

        public async Task CommitTransactAsync(IDbContextTransaction dbContextTransaction)
            => await dbContextTransaction.CommitAsync();

        public async Task OpenConnectAsync()
            => await _context.Database.OpenConnectionAsync();

        public async Task RollBackTransactionAsync(IDbContextTransaction dbContextTransaction)
            => await dbContextTransaction.RollbackAsync();
    }
}
