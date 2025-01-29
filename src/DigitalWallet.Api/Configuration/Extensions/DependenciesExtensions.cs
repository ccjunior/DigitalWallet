using DigitalWallet.Application.Services;
using DigitalWallet.Application.Services.Impl;
using DigitalWallet.Data.Helper;
using DigitalWallet.Data.Repository;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace DigitalWallet.Api.Configuration.Extensions
{
    /// <summary>
    /// Classe de configuração do depêndencias da aplicação.
    /// </summary>
    public static class DependenciesExtensions
    {
        /// <summary>
        /// Configuração das dependencias (Serrvices, Repository, etc...).
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDependencies(
            this IServiceCollection services, IConfiguration configurations)
        {
            services
                // Others    
                .AddSingleton(serviceProvider => configurations)
                .AddScoped<IDatabaseMigrator, DatabaseMigrator>()
                // Services
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IWalletService, WalletService>()
                .AddTransient<ITransactionService, TransactionService>()
                // Repository
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IWalletRepository, WalletRepository>()
                .AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
