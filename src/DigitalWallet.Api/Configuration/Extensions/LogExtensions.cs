using Serilog.Events;
using Serilog;

namespace DigitalWallet.Api.Configuration.Extensions
{
    public static class LogExtensions
    {
        /// <summary>
        /// Configuração de Logs do sistema.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configurations)
        {
            string connectionString = configurations.GetConnectionString("DefaultConnection");

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithMachineName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .WriteTo.Console()
                    .WriteTo.MySQL(connectionString)
                    .CreateLogger();

            return services;
        }
    }
}
