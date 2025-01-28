namespace DigitalWallet.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }


        public Wallet(Guid userId)
        {
            UserId = userId;
            Balance = 0;
               
        }
        public Wallet()
        {
            Transactions = new List<Transaction>();
        }

        public void AddBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Balance += amount;
        }

        public void RemoveBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
        }
    }
}
