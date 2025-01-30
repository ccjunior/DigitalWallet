using DigitalWallet.Data.Context;
using DigitalWallet.Domain.Common;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Data.Helper
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            if (!context.Users.Any())
            {
                // Usuários
                var user1 = new User("John Doe", "john@example.com", PasswordHasher.HashPassword("hashedpassword1"));
                var user2 = new User("Jane Doe", "jane@example.com", PasswordHasher.HashPassword("hashedpassword2"));

                // Carteiras associadas aos usuários
                var wallet1 = user1.Wallet;
                wallet1.AddBalance(1000); // Saldo inicial
                wallet1.Transactions.Add(new Transaction(wallet1.Id, TransactionType.Deposit, 1000, "Depósito inicial"));

                var wallet2 = user2.Wallet;
                wallet2.AddBalance(2000); // Saldo inicial
                wallet2.Transactions.Add(new Transaction(wallet2.Id, TransactionType.Deposit, 2000, "Depósito inicial"));

                // Adiciona os usuários ao contexto
                context.Users.AddRange(user1, user2);

                // Adiciona as carteiras ao contexto (opcional, pois estão relacionadas aos usuários)
                context.Wallets.AddRange(wallet1, wallet2);

                // Transações já estão associadas às carteiras
            }

            await context.SaveChangesAsync();
        }
    }
}
