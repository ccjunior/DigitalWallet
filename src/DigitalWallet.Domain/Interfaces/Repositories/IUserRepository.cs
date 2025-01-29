using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
