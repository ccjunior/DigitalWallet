using DigitalWallet.Domain.Enum;

namespace DigitalWallet.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public TransactionType TransactionType { get; set; } // Deposit, Withdraw, Transfer
        public decimal Amount { get; set; }
        public string Description { get; set; }


        public Transaction(Guid walletId, TransactionType transactionType, decimal amount, string description)
        {
            WalletId = walletId;
            TransactionType = transactionType;
            Amount = amount;
            Description = description;
        }

        public void SetDate()
        {
            DateCreated = DateTime.UtcNow;
        }

        //public Guid FromWalletId { get; private set; }
        //public Guid ToWalletId { get; private set; }
        //public decimal Amount { get; private set; }
        //public TransactionStatus Status { get; private set; }

        //public Transaction(Guid fromWalletId, Guid toWalletId, decimal amount)
        //{
        //    FromWalletId = fromWalletId;
        //    ToWalletId = toWalletId;
        //    Amount = amount;
        //    Status = TransactionStatus.Pending;
        //}

        //public void Complete()
        //{
        //    Status = TransactionStatus.Completed;
        //}

        //public void Fail()
        //{
        //    Status = TransactionStatus.Failed;
        //}
    }
}
