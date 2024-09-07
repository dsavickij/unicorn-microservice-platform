#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["core/services/system-status/Unicorn.Core.Services.SystemStatus.HealthCheck/Unicorn.Core.Services.SystemStatus.HealthCheck.csproj", "core/services/system-status/Unicorn.Core.Services.SystemStatus.HealthCheck/"]
RUN dotnet restore "core/services/system-status/Unicorn.Core.Services.SystemStatus.HealthCheck/Unicorn.Core.Services.SystemStatus.HealthCheck.csproj"
COPY . .
WORKDIR "/src/core/services/system-status/Unicorn.Core.Services.SystemStatus.HealthCheck"
RUN dotnet build "Unicorn.Core.Services.SystemStatus.HealthCheck.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Core.Services.SystemStatus.HealthCheck.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Core.Services.SystemStatus.HealthCheck.dll"]