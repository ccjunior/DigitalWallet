using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Dtos.Response;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using Serilog;

namespace DigitalWallet.Application.Services.Impl
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<MyDbContext> _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, IUnitOfWork<MyDbContext> unitOfWork)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<UserBalanceResponse>> GetWalletByUserIdAsync(Guid userId)
        {
           var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return ServiceResult<UserBalanceResponse>.ErrorResult("Usuário não encontrado.");

            var wallet = await _walletRepository.GetByUserIdAsync(userId); 
            var userBalanceResponse = new UserBalanceResponse(user.Id, user.Name, wallet.Id, wallet.Balance);

            return ServiceResult<UserBalanceResponse>.SuccessResult(userBalanceResponse);
        }
    }
}
