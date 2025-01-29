namespace DigitalWallet.Api.Configuration.Extensions
{
    // <summary>
    /// Classe de configuração do Cors da aplicação.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Configuração dos cors aceitos.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
            => services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((host) => true).AllowCredentials();
                });
            });
    }
}
