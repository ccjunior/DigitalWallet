using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DigitalWallet.Api.Configuration.Extensions
{
    public static class JwtConfigurationExtension
    {
        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations["Jwt:Key"])),
                    ValidIssuer = configurations["Jwt:Issuer"],
                    ValidAudience = configurations["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
            
            return services;
        }
    }
}
