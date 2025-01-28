using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Data.Repository
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<Wallet> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
