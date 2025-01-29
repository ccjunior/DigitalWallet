using DigitalWallet.Data.Context;
using DigitalWallet.Data.Repository;
using DigitalWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace DigitalWallet.Tests.Data.Repositories
{
    public class WalletRepositoryTests
    {
        private MyDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MyDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveWallet()
        {
            var dbContext = GetDbContext();
            var repository = new WalletRepository(dbContext);
            var wallet = new Wallet(Guid.NewGuid());
            wallet.AddBalance(1000m);

            await repository.AddAsync(wallet);
            var walletFromDb = await dbContext.Wallets.FindAsync(wallet.Id);

            Assert.NotNull(walletFromDb);
            Assert.Equal(1000, walletFromDb.Balance);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnCorrectWallet()
        {
            var dbContext = GetDbContext();
            var repository = new WalletRepository(dbContext);
            var userId = Guid.NewGuid();
            var wallet = new Wallet(Guid.NewGuid());
            wallet.AddBalance(500m);
            await dbContext.Wallets.AddAsync(wallet);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new WalletRepository(dbContext);

            var result = await repository.GetByUserIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
