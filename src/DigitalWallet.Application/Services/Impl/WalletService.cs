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

        public async Task AddFundsAsync(Guid userId, decimal amount)
        {
           var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                wallet = new Wallet(userId);
                await _walletRepository.AddAsync(wallet);
            }
            else
            {
                wallet = new Wallet(userId);
                wallet.AddBalance(amount);
                await _walletRepository.UpdateAsync(wallet);
            }
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

        public async Task TransferFundsAsync(Guid fromUserId, Guid toUserId, decimal amount)
        {
            Log.Information(
            $"[LOG INFORMATION] - SET TITLE {nameof(WalletService)} - METHOD {nameof(TransferFundsAsync)}\n");

            var fromWallet = await _walletRepository.GetByUserIdAsync(fromUserId);
            var toWallet = await _walletRepository.GetByUserIdAsync(toUserId);

            if (fromWallet == null || toWallet == null)
                throw new Exception("Invalid sender or receiver.");

            if (fromWallet.Balance < amount)
                throw new Exception("Insufficient balance.");

            fromWallet.RemoveBalance(amount);
            toWallet.AddBalance(amount);

            var transaction = await _unitOfWork.BeginTransactAsync();

            try
            {

                await _walletRepository.UpdateAsync(fromWallet);
                await _walletRepository.UpdateAsync(toWallet);


                
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();

            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            }
        }
    }
}
