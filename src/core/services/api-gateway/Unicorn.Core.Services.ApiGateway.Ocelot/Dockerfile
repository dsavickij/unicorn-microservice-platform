#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["core/services/api-gateway/Unicorn.Core.Services.ApiGateway.Ocelot/Unicorn.Core.Services.ApiGateway.Ocelot.csproj", "core/services/api-gateway/Unicorn.Core.Services.ApiGateway.Ocelot/"]
RUN dotnet restore "core/services/api-gateway/Unicorn.Core.Services.ApiGateway.Ocelot/Unicorn.Core.Services.ApiGateway.Ocelot.csproj"
COPY . .
WORKDIR "/src/core/services/api-gateway/Unicorn.Core.Services.ApiGateway.Ocelot"
RUN dotnet build "Unicorn.Core.Services.ApiGateway.Ocelot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Core.Services.ApiGateway.Ocelot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Core.Services.ApiGateway.Ocelot.dll"]