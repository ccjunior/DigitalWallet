using DigitalWallet.Api.Configuration;
using DigitalWallet.Api.Configuration.Extensions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;


try
{
    var builder = WebApplication.CreateBuilder(args);
    var configurations = builder.Configuration;


    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    /// <sumary>
    /// Configura as configurações de inicialização da aplicação.
    /// </sumary>
    builder.Services
        .ConfigureSerilog(configurations)
        .AddHttpContextAccessor()
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .ConfigureDependencies(configurations)
        .AddDatabase(configurations)
        .ConfigureCors()
        .AddControllers(options =>
        {
            options.EnableEndpointRouting = false;
            options.Filters.Add(new ProducesAttribute("application/json"));

        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Services
              .ConfigureSwagger(configurations);


    builder.Services.AddAuthorization();

    builder.Services.AddSingleton<JwtTokenGeneratorConfiguration>();

    var applicationbuilder = builder.Build();

    applicationbuilder
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseHsts()
            .UseCors()
            .UseResponseCaching()
            .UseSwaggerConfigurations(configurations)
            .UseAuthentication()
            .UseAuthorization()
            .ApplyMigrations();
           
    applicationbuilder.MapControllers();


    applicationbuilder
       .Lifetime.ApplicationStarted
           .Register(() => Log.Debug(
                   $"[LOG DEBUG] - Aplicação inicializada com sucesso: [DigitalWallet.API]\n"));

    applicationbuilder.Run();
}
catch (Exception exception)
{
    Log.Error($"[LOG ERROR] - Ocorreu um erro ao inicializar a aplicacao [DigitalWallet.API] - {exception.Message}\n"); throw;
}



