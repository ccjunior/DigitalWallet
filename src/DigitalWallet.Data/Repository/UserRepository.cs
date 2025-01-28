using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                    .Include(u => u.Wallet)
                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetWithWalletAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
