using DigitalWallet.Api;
using DigitalWallet.Api.Configuration;
using DigitalWallet.Api.Configuration.Extensions;
using DigitalWallet.Api.Provider;
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


    builder.Services.AddAuthorization();

    // Configuração do JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
                //ValidateIssuer = true,
                //ValidateAudience = true,
                //ValidateLifetime = true,
                //ValidateIssuerSigningKey = true,
               
            };
        });

   

    builder.Services.AddSingleton<TokenProvider>();

    builder.Services
        //.ConfigureJwt(configurations)
        //.ConfigureSerilog(configurations)
        //.AddHttpContextAccessor()
        .AddEndpointsApiExplorer()
        //.AddSwaggerGen()
        .ConfigureDependencies(configurations)
        .AddDatabase(configurations);
    //.ConfigureCors()
    //.AddControllers(options =>
    //{
    //    options.EnableEndpointRouting = false;
    //    options.Filters.Add(new ProducesAttribute("application/json"));

    //}).AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //});

    builder.Services.AddControllers();


    

    builder.Services
              .ConfigureSwagger(configurations);

   


    var applicationbuilder = builder.Build();

    applicationbuilder
            //.UseHttpsRedirection()
            //.UseStaticFiles()
            //.UseCookiePolicy()
            //.UseHsts()
            //.UseCors()
            //.UseResponseCaching()
            .UseAuthorizationDebug()
            .UseSwaggerConfigurations(configurations)
            .UseRouting()
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



