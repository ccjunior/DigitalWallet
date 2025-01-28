using DigitalWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWallet.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.HasKey(p => p.Id);

            builder.HasOne(u => u.Wallet)
                   .WithOne()
                   .HasForeignKey<Wallet>(w => w.UserId);
        }
    }
}
