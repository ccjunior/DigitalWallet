using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Data.Helper
{
    public class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly MyDbContext _context;

        public DatabaseMigrator(MyDbContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }
    }
}
