using Microsoft.OpenApi.Models;

namespace DigitalWallet.Api.Configuration.Extensions
{
    /// <summary>
    /// Classe de configuração do Swagger da aplicação.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Configuração do swagger do sistema.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurations"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });

            return services;
        }

        /// <summary>
        /// Configuração de uso do swagger do sistema.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="configurations"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerConfigurations(this IApplicationBuilder application, IConfiguration configurations)
        {
            var apiVersion = "v1";

            application.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            application
                .UseSwaggerUI(swagger =>
                {
                    swagger.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{apiVersion}");
                });

            application
                .UseMvcWithDefaultRoute();

            return application;
        }
    }
}
