using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Domain.Dtos.Request;
using DigitalWallet.Domain.Dtos.Response;

namespace DigitalWallet.Application.Services
{
    public interface IUserService
    {
        Task<ServiceResult<UserResponseDto>> CreateUserAsync(RegisterRequest dto);
        Task<ServiceResult<UserResponseDto>> GetUserByIdAsync(Guid userId);
        Task<ServiceResult<UserFullResponseDto>> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserFullResponseDto>> GetUsers();
    }
}
