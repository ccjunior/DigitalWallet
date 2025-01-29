using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Common;
using DigitalWallet.Domain.Dtos.Request;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Reflection;

namespace DigitalWallet.Tests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUnitOfWork<MyDbContext>> _unitOfWorkMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork<MyDbContext>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _walletRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated()
        {
            var registerRequest = new RegisterRequest("joao andrade", "joao@example.com", "password123");

            var createdUser = new User(registerRequest.Name, registerRequest.Email, PasswordHasher.HashPassword(registerRequest.PasswordHash));
            var wallet = new Wallet(createdUser.Id);

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User)null); 

            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(createdUser);

            _walletRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Wallet>()))
                .ReturnsAsync((Wallet wallet) => wallet);

            _unitOfWorkMock.Setup(uow => uow.BeginTransactAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var result = await _userService.CreateUserAsync(registerRequest);

            Assert.True(result.Success);
            Assert.Equal(registerRequest.Email, result.Data.Email);
            Assert.Equal("joao andrade", result.Data.Name);
            Assert.Equal(0, result.Data.Balance);

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            _walletRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Wallet>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnError_WhenEmailIsAlreadyInUse()
        {
            var registerRequest = new RegisterRequest("joao andrade", "joao@example.com", "password123");

            var existingUser = new User("Existing User", registerRequest.Email, "hashedpassword");

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync(existingUser); 

            var result = await _userService.CreateUserAsync(registerRequest);

            Assert.False(result.Success);
            Assert.Equal("O email já está em uso", result.Message);

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            _walletRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Wallet>()), Times.Never);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnSuccess_WhenUserExists()
        {
            var email = "joao@example.com";
            var user = new User("joao andrade", email, "hashedpassword");

            var wallet = new Wallet(user.Id);
            wallet.AddBalance(100m);

            user.SetWallet(wallet);

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
                .ReturnsAsync(user);

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            Assert.Equal(email, result.Data.Email);
            Assert.Equal(100m, result.Data.Balance);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnSuccess_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User("joao andrade", "joao@example.com", "hashedpassword");
            var wallet = new Wallet(user.Id);
            wallet.AddBalance(200m);

            user.SetWallet(wallet);

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("joao andrade", result.Data.Name);
            Assert.Equal(200m, result.Data.Balance);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnAllUsers_WhenUsersExist()
        {
            var users = new List<User>
            {
               CreateUser("joao andrade", "joao@example.com", "hashed1", 100m),
               CreateUser("Jane", "jane@example.com", "hashed2", 200m)
            };

            _userRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);

            var result = await _userService.GetUsers();

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        User CreateUser(string name, string email, string passwordHash, decimal balance)
        {
            var user = new User(name, email, passwordHash);
            var wallet = new Wallet(user.Id);
            wallet.AddBalance(balance);
            
            typeof(User).GetProperty("Wallet", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(user, wallet);

            return user;
        }

    }
}
