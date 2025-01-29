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

        public async Task<IEnumerable<Transaction>> GetTransfersByWalletIdAsync(Guid walletId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Transactions.AsQueryable();

            query = query.Where(t => t.WalletId == walletId && t.TransactionType == Domain.Enum.TransactionType.Transfer);

            if (startDate.HasValue)
                query = query.Where(t => t.DateCreated >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.DateCreated <= endDate.Value);

            return await query.ToListAsync();
        }
    }
}
