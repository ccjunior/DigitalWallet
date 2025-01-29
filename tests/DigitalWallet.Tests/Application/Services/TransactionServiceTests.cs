using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enum;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace DigitalWallet.Tests.Application.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUnitOfWork<MyDbContext>> _unitOfWorkMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork<MyDbContext>>();

            _transactionService = new TransactionService(
                _walletRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task DepositAsync_ShouldReturnSuccess_WhenDepositIsValid()
        {
            var walletId = Guid.NewGuid();
            var amount = 100m;
            var wallet = new Wallet(walletId);
            wallet.AddBalance(10m);

            _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(walletId))
                .ReturnsAsync(wallet);

            _unitOfWorkMock.Setup(uow => uow.BeginTransactAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var result = await _transactionService.DepositAsync(walletId, amount);

            Assert.True(result.Success);
            Assert.Equal("Depósito realizado com sucesso.", result.Message);
            Assert.Equal(amount, wallet.Balance);

            _walletRepositoryMock.Verify(repo => repo.UpdateAsync(wallet), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldReturnError_WhenBalanceIsInsufficient()
        {
            var walletId = Guid.NewGuid();
            var amount = 100m;
            var wallet = new Wallet(walletId);
            wallet.AddBalance(50m);

            _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(walletId))
                .ReturnsAsync(wallet);

            var result = await _transactionService.WithdrawAsync(walletId, amount);

            Assert.False(result.Success);
            Assert.Equal("Saldo insuficiente.", result.Message);

            _walletRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Wallet>()), Times.Never);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task TransferAsync_ShouldReturnSuccess_WhenTransferIsValid()
        {
            var fromWalletId = Guid.NewGuid();
            var toWalletId = Guid.NewGuid();
            var amount = 50m;

            var fromWallet = new Wallet(fromWalletId);
            fromWallet.AddBalance(100m);

            var toWallet = new Wallet(toWalletId);

            _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(fromWalletId))
                .ReturnsAsync(fromWallet);

            _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(toWalletId))
                .ReturnsAsync(toWallet);

            _unitOfWorkMock.Setup(uow => uow.BeginTransactAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var result = await _transactionService.TransferAsync(fromWalletId, toWalletId, amount);

            Assert.True(result.Success);
            Assert.Equal("Transferência realizada com sucesso.", result.Message);

            Assert.Equal(50m, fromWallet.Balance); // Saldo reduzido na carteira de origem
            Assert.Equal(50m, toWallet.Balance);  // Saldo aumentado na carteira de destino

            _walletRepositoryMock.Verify(repo => repo.UpdateAsync(fromWallet), Times.Once);
            _walletRepositoryMock.Verify(repo => repo.UpdateAsync(toWallet), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetTransfersAsync_ShouldReturnTransfers_WhenTransfersExist()
        {
            var walletId = Guid.NewGuid();

            var transfers = new List<Transaction>
        {
            new Transaction(walletId, TransactionType.Transfer, 100m, "Transferência para carteira X"),
            new Transaction(walletId, TransactionType.Transfer, 50m, "Transferência para carteira Y")
        };

            _transactionRepositoryMock.Setup(repo => repo.GetTransfersByWalletIdAsync(walletId, null, null))
                .ReturnsAsync(transfers);

            var result = await _transactionService.GetTransfersAsync(walletId);

            Assert.True(result.Success);

            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetTransfersAsync_ShouldReturnError_WhenNoTransfersExist()
        {
            var walletId = Guid.NewGuid();

            _transactionRepositoryMock.Setup(repo => repo.GetTransfersByWalletIdAsync(walletId, null, null))
                .ReturnsAsync(new List<Transaction>());

            var result = await _transactionService.GetTransfersAsync(walletId);

            Assert.False(result.Success);
            Assert.Equal("Nenhuma transferência encontrada para os critérios especificados.", result.Message);
        }
    }

}
