using DigitalWallet.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Api.Configuration.Extensions
{
    /// <summary>
    /// Classe de extensão de configuração de dataBase na inicialização da aplicação.
    /// </summary>
    public static class DataBaseExtensions
    {
        /// <summary>
        /// Executa a config da Base.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<MyDbContext>();

            return services;
        }
    }
}
