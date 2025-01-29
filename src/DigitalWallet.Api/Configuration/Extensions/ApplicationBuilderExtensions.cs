using DigitalWallet.Domain.Interfaces;

namespace DigitalWallet.Api.Configuration.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbMigrator = services.ServiceProvider.GetService<IDatabaseMigrator>();

            dbMigrator?.Migrate();
        }
    }
}
