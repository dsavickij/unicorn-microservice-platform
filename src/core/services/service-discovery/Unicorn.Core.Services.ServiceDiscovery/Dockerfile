#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["core/services/service-discovery/Unicorn.Core.Services.ServiceDiscovery/Unicorn.Core.Services.ServiceDiscovery.csproj", "core/services/service-discovery/Unicorn.Core.Services.ServiceDiscovery/"]
RUN dotnet restore "core/services/service-discovery/Unicorn.Core.Services.ServiceDiscovery/Unicorn.Core.Services.ServiceDiscovery.csproj"
COPY . .
WORKDIR "/src/core/services/service-discovery/Unicorn.Core.Services.ServiceDiscovery"
RUN dotnet build "Unicorn.Core.Services.ServiceDiscovery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Core.Services.ServiceDiscovery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Core.Services.ServiceDiscovery.dll"]