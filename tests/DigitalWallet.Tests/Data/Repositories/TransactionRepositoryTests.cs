using DigitalWallet.Data.Context;
using DigitalWallet.Data.Repository;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Tests.Data.Repositories
{
    public class TransactionRepositoryTests
    {
        private MyDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MyDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveTransaction()
        {
            var dbContext = GetDbContext();
            var repository = new TransactionRepository(dbContext);
            var transaction = new Transaction(Guid.NewGuid(), TransactionType.Deposit, 500, "Depósito");

            await repository.AddAsync(transaction);
            var transactionFromDb = await dbContext.Transactions.FindAsync(transaction.Id);

            Assert.NotNull(transactionFromDb);
            Assert.Equal(500, transactionFromDb.Amount);
            Assert.Equal(TransactionType.Deposit, transactionFromDb.TransactionType);
        }

        [Fact]
        public async Task GetTransfersByWalletIdAsync_ShouldReturnOnlyTransfers()
        {
            var dbContext = GetDbContext();
            var repository = new TransactionRepository(dbContext);
            var walletId = Guid.NewGuid();

            var deposit = new Transaction(walletId, TransactionType.Deposit, 100, "Depósito");
            var transfer1 = new Transaction(walletId, TransactionType.Transfer, 200, "Transferência 1");
            var transfer2 = new Transaction(walletId, TransactionType.Transfer, 300, "Transferência 2");

            await dbContext.Transactions.AddRangeAsync(deposit, transfer1, transfer2);
            await dbContext.SaveChangesAsync();

            var transfers = await repository.GetTransfersByWalletIdAsync(walletId);

            Assert.Equal(2, transfers.Count());
            Assert.All(transfers, t => Assert.Equal(TransactionType.Transfer, t.TransactionType));
        }

        [Fact]
        public async Task GetTransfersByWalletIdAsync_ShouldFilterByDateRange()
        {
            var dbContext = GetDbContext();
            var repository = new TransactionRepository(dbContext);
            var walletId = Guid.NewGuid();

            var oldTransaction = new Transaction(walletId, TransactionType.Transfer, 100, "Antiga")
            {
                DateCreated = DateTime.UtcNow.AddDays(-10)
            };
            var recentTransaction = new Transaction(walletId, TransactionType.Transfer, 200, "Recente")
            {
                DateCreated = DateTime.UtcNow.AddDays(-2)
            };

            await dbContext.Transactions.AddRangeAsync(oldTransaction, recentTransaction);
            await dbContext.SaveChangesAsync();

            var transfers = await repository.GetTransfersByWalletIdAsync(walletId, DateTime.UtcNow.AddDays(-5));

            Assert.Single(transfers);
            Assert.Equal(200, transfers.First().Amount);
        }
    }
}
