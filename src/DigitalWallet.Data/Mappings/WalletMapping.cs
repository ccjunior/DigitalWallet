using DigitalWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWallet.Data.Mappings
{
    public class WalletMapping : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(w => w.Balance)
                   .HasPrecision(18, 2);
        }
    }
}
