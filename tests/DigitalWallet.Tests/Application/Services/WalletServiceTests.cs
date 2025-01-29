using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using Moq;

namespace DigitalWallet.Tests.Application.Services
{
    public class WalletServiceTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly WalletService _walletService;

        public WalletServiceTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletService = new WalletService(_walletRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWalletByUserIdAsync_ShouldReturnError_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _walletService.GetWalletByUserIdAsync(userId);

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetWalletByUserIdAsync_ShouldReturnWallet_WhenUserExists()
        {
            var user = new User("Jose", "jose@example.com", "hashedpassword");
            var wallet = new Wallet(user.Id);
            wallet.AddBalance(500m);

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _walletRepositoryMock.Setup(repo => repo.GetByUserIdAsync(user.Id)).ReturnsAsync(wallet);

            var result = await _walletService.GetWalletByUserIdAsync(user.Id);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Id, result.Data.UserId);
            Assert.Equal("Jose", result.Data.Name);
            Assert.Equal(wallet.Id, result.Data.WalletId);
            Assert.Equal(500m, result.Data.Balance);
        }
    }
}
