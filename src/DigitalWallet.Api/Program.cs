using DigitalWallet.Api.Configuration.Extensions;
using DigitalWallet.Api.Provider;
using DigitalWallet.Data.Helper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configurations = builder.Configuration;

    builder.Services.AddAuthorization();

    builder.Services.AddSingleton<TokenProvider>();

    builder.Services
        .ConfigureJwt(configurations)
        .ConfigureSerilog(configurations)
        .AddHttpContextAccessor()
        .AddEndpointsApiExplorer()
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

    var applicationbuilder = builder.Build();

  

    applicationbuilder
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseHsts()
            .UseCors()
            .UseResponseCaching()
            .UseSwaggerConfigurations(configurations)
            .UseRouting()
            .UseAuthentication()  
            .UseAuthorization()   
            .ApplyMigrations();

    applicationbuilder.MapControllers();

    // Chama o seed para popular o banco de dados
    using (var scope = applicationbuilder.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            await DatabaseSeeder.SeedAsync(services);
        }
        catch (Exception ex)
        {
            Log.Error($"[LOG ERROR] - Ocorreu um erro ao popular o banco de dados. [DigitalWallet.API] - {ex.Message}\n");
            throw;
        }
    }

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



