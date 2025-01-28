namespace DigitalWallet.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public Wallet Wallet { get; private set; }

        private User() { } 

        public User(string name, string email, string passwordHash)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Wallet = new Wallet();
        }
    }
}
