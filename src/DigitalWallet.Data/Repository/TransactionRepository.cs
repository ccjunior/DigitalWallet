using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Data.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MyDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.DateCreated)
                .ToListAsync();
        }

        //public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, DateTime? startDate, DateTime? endDate)
        //{
        //    var wallet = await _context.Wallets
        //        .FirstOrDefaultAsync(w => w.UserId == userId);

        //    if (wallet == null)
        //        return Enumerable.Empty<Transaction>();

        //    var query = _context.Transactions
        //        .Where(t => t.FromWalletId == wallet.Id || t.ToWalletId == wallet.Id);

        //    if (startDate.HasValue)
        //        query = query.Where(t => t.DateCreated >= startDate.Value);

        //    if (endDate.HasValue)
        //        query = query.Where(t => t.DateCreated <= endDate.Value);

        //    return await query
        //        .OrderByDescending(t => t.DateCreated)
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId)
        //{
        //    return await _context.Transactions
        //        .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId)
        //        .OrderByDescending(t => t.DateCreated)
        //        .ToListAsync();
        //}
    }
}
