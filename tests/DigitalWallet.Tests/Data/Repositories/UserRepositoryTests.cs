using DigitalWallet.Data.Context;
using DigitalWallet.Data.Repository;
using DigitalWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Tests.Data.Repositories
{
    public class UserRepositoryTests
    {
        private MyDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MyDbContext(options);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);
            var user = new User("Test User", "test@example.com", "hashedpassword");
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);
            var user = new User("Test User", "test@example.com", "hashedpassword");
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByEmailAsync(user.Email);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var result = await repository.GetByEmailAsync("notfound@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);
            var users = new List<User>
        {
            new User("User One", "one@example.com", "hashedpassword"),
            new User("User Two", "two@example.com", "hashedpassword")
        };
            await dbContext.Users.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
