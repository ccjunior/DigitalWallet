using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Common;
using DigitalWallet.Domain.Dtos.Request;
using DigitalWallet.Domain.Dtos.Response;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using Serilog;

namespace DigitalWallet.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork<MyDbContext> unitOfWork;

        public UserService(IUserRepository userRepository, IWalletRepository walletRepository, IUnitOfWork<MyDbContext> unitOfWork)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<UserResponseDto>> CreateUserAsync(RegisterRequest dto)
        {
            Log.Information(
             $"[LOG INFORMATION] - SET TITLE {nameof(UserService)} - METHOD {nameof(CreateUserAsync)}\n");


            var user = await GetUser(dto.Email);
            if (user != null)
                return ServiceResult<UserResponseDto>.ErrorResult("O email já está em uso");

            var transaction = await unitOfWork.BeginTransactAsync();

            try
            {
                var newUser = new User(
                 name: dto.Name,
                 email: dto.Email,
                 passwordHash: PasswordHasher.HashPassword(dto.PasswordHash));

                user = await _userRepository.AddAsync(newUser);

                var wallet = new Wallet(user.Id);
                await _walletRepository.AddAsync(wallet);

                await unitOfWork.CommitAsync();
                await transaction.CommitAsync();

                var UserResponse = new UserResponseDto(user.Id, user.Name, user.Email, wallet.Balance);

                return ServiceResult<UserResponseDto>.SuccessResult(UserResponse);
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
                return ServiceResult<UserResponseDto>.ErrorResult("Falha ao cadastrar o usuário");
            }
        }

        public async Task<ServiceResult<UserFullResponseDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var userResponse = new UserFullResponseDto(user.Id, user.Name, user.Email, user.PasswordHash, user.DateCreated,user.Wallet.Balance);
            return ServiceResult<UserFullResponseDto>.SuccessResult(userResponse);
        }

        public async Task<ServiceResult<UserResponseDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var userResponse = new UserResponseDto(user.Id, user.Name, user.Email, user.Wallet.Balance);
            return ServiceResult<UserResponseDto>.SuccessResult(userResponse);
        }

        private async Task<User> GetUser(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}
