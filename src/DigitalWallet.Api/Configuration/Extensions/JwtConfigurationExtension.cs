using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DigitalWallet.Api.Configuration.Extensions
{
    public static class JwtConfigurationExtension
    {
        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configurations)
        {
            var key = configurations["Jwt:Key"];
            var issuer = configurations["Jwt:Issuer"];
            var audience = configurations["Jwt:Audience"];

            Console.WriteLine($"Configuring JWT - Key: {key}, Issuer: {issuer}, Audience: {audience}");


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
                {
                    bearerOptions.RequireHttpsMetadata = true;
                    bearerOptions.SaveToken = true;
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configurations["Jwt:Issuer"],
                        ValidAudience = configurations["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations["Jwt:Key"]))
                    };

                    // Criando um provedor de logs
                    var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                    var logger = loggerFactory.CreateLogger("JwtBearerEvents");

                    bearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            logger.LogError($"[JWT ERROR] Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            //context.HttpContext.Request.EnableBuffering();

                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogInformation("[JWT SUCCESS] Token successfully validated.");

                            await Task.CompletedTask;
                        }
                    };
                });




            return services;
        }
    }
}
