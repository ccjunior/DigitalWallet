using DigitalWallet.Domain.Dtos.Response;
using DigitalWallet.Domain.Interfaces.Repositories;

namespace DigitalWallet.Application.Services.Impl
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
        }

        public Task AddFundsAsync(Guid userId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserBalanceResponse>> GetBalanceAsync(Guid userId)
        {
           var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return ServiceResult<UserBalanceResponse>.ErrorResult("Usuário não encontrado.");

            var wallet = await _walletRepository.GetByUserIdAsync(userId); 
            var userBalanceResponse = new UserBalanceResponse(user.Id, user.Name, wallet.Balance);

            return ServiceResult<UserBalanceResponse>.SuccessResult(userBalanceResponse);
        }

        public Task TransferFundsAsync(Guid fromUserId, Guid toUserId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
