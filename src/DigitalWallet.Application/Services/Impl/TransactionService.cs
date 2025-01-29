using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Dtos;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enum;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using Serilog;

namespace DigitalWallet.Application.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork<MyDbContext> _unitOfWork;

        public TransactionService(IWalletRepository walletRepository, ITransactionRepository transactionRepository, IUnitOfWork<MyDbContext> unitOfWork)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<TransctionDto>> DepositAsync(Guid walletId, decimal amount)
        {
            Log.Information(
                     $"[LOG INFORMATION] - SET TITLE {nameof(TransactionService)} - METHOD {nameof(DepositAsync)}\n");

            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                return ServiceResult<TransctionDto>.ErrorResult("Carteira não encontrada.");

            wallet.AddBalance(amount);

            var transaction = await _unitOfWork.BeginTransactAsync();

            try
            {
                await _walletRepository.UpdateAsync(wallet);

                var transactionWallet = new Transaction(wallet.Id, TransactionType.Deposit, amount, "Depósito realizado");

                await _transactionRepository.AddAsync(transactionWallet);

                await _unitOfWork.CommitAsync();
                
                await transaction.CommitAsync();

                var transactionDto = new TransctionDto(transactionWallet.Id, TransactionType.Deposit, amount);

                return ServiceResult<TransctionDto>.SuccessResult(transactionDto, "Depósito realizado com sucesso.");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
                throw;
            }
        }

        public async Task<ServiceResult<TransctionDto>> WithdrawAsync(Guid walletId, decimal amount)
        {
            Log.Information(
                   $"[LOG INFORMATION] - SET TITLE {nameof(TransactionService)} - METHOD {nameof(WithdrawAsync)}\n");

            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                return ServiceResult<TransctionDto>.ErrorResult("Carteira não encontrada.");

            if (wallet.Balance < amount)
                return ServiceResult<TransctionDto>.ErrorResult("Saldo insuficiente.");

            wallet.RemoveBalance(amount);

            var transaction = await _unitOfWork.BeginTransactAsync();

            try
            {
                await _walletRepository.UpdateAsync(wallet);

                var transactionWallet = new Transaction(wallet.Id, TransactionType.Withdraw, amount, "Saque realizado");

                await _transactionRepository.AddAsync(transactionWallet);

                await _unitOfWork.CommitAsync();
                
                await transaction.CommitAsync();

                var transactionDto = new TransctionDto(transactionWallet.Id, TransactionType.Deposit, amount);

                return ServiceResult<TransctionDto>.SuccessResult(transactionDto, "Saque realizado com sucesso.");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
                throw;
            }
        }

        public async Task<ServiceResult<TransctionDto>> TransferAsync(Guid fromWalletId, Guid toWalletId, decimal amount)
        {
            var fromWallet = await _walletRepository.GetByIdAsync(fromWalletId);
            var toWallet = await _walletRepository.GetByIdAsync(toWalletId);

            if (fromWallet == null || toWallet == null)
                return ServiceResult<TransctionDto>.ErrorResult("Uma ou ambas as carteiras não foram encontradas.");

            if (fromWallet.Balance < amount)
                return ServiceResult<TransctionDto>.ErrorResult("Saldo insuficiente na carteira de origem.");

            fromWallet.RemoveBalance(amount);
            toWallet.AddBalance(amount);


            var transaction = await _unitOfWork.BeginTransactAsync();

            try
            {
                await _walletRepository.UpdateAsync(fromWallet);
                
                await _walletRepository.UpdateAsync(toWallet);

                var transactionOut = new Transaction(fromWallet.Id, TransactionType.Transfer, amount, $"Transferência enviada para a carteira {toWallet.Id}");
                
                var transactionIn = new Transaction(toWallet.Id, TransactionType.Transfer, amount, $"Transferência recebida da carteira {fromWallet.Id}");

                await _transactionRepository.AddAsync(transactionOut);

                await _transactionRepository.AddAsync(transactionIn);

                await _unitOfWork.CommitAsync();

                await transaction.CommitAsync();

                var transactionDto = new TransctionDto(transactionOut.Id, TransactionType.Deposit, amount);
                
                return ServiceResult<TransctionDto>.SuccessResult(transactionDto, "Transferência realizada com sucesso.");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
                throw;
            }
        }
    }
}
