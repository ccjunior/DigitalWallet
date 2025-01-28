namespace DigitalWallet.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
        }
    }
}
