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
                    .Include(u => u.Wallet)
                    .FirstOrDefaultAsync(u => u.Email == email);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                    .Include(u => u.Wallet)
                    .ToListAsync(); 
        }
    }
}
