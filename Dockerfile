FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DigitalWallet.Api/DigitalWallet.Api.csproj", "src/DigitalWallet.Api/"]
COPY ["src/DigitalWallet.Data/DigitalWallet.Data.csproj", "src/DigitalWallet.Data/"]
COPY ["src/DigitalWallet.Domain/DigitalWallet.Domain.csproj", "src/DigitalWallet.Domain/"]
COPY ["src/DigitalWallet.Application/DigitalWallet.Application.csproj", "src/DigitalWallet.Application/"]
RUN dotnet restore "./src/DigitalWallet.Api/DigitalWallet.Api.csproj"
COPY . .
WORKDIR "/src/src/DigitalWallet.Api"
RUN dotnet build "./DigitalWallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DigitalWallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalWallet.Api.dll"]