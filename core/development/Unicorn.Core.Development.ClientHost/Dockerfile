#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["core/development/Unicorn.Core.Development.ClientHost/Unicorn.Core.Development.ClientHost.csproj", "core/development/Unicorn.Core.Development.ClientHost/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.HostConfiguration.SDK/Unicorn.Core.Infrastructure.HostConfiguration.SDK.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.HostConfiguration.SDK/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.Communication.Grpc.SDK/Unicorn.Core.Infrastructure.Communication.Grpc.SDK.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.Communication.Grpc.SDK/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.Communication.Common/Unicorn.Core.Infrastructure.Communication.Common.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.Communication.Common/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.Communication.MessageBroker/Unicorn.Core.Infrastructure.Communication.MessageBroker.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.Communication.MessageBroker/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.Communication.Http.SDK/Unicorn.Core.Infrastructure.Communication.Http.SDK.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.Communication.Http.SDK/"]
COPY ["core/infrastructure/Unicorn.Core.Infrastructure.Security.IAM/Unicorn.Core.Infrastructure.Security.IAM.csproj", "core/infrastructure/Unicorn.Core.Infrastructure.Security.IAM/"]
COPY ["core/development/Unicorn.Core.Development.ServiceHost.SDK/Unicorn.Core.Development.ServiceHost.SDK.csproj", "core/development/Unicorn.Core.Development.ServiceHost.SDK/"]
RUN dotnet restore "core/development/Unicorn.Core.Development.ClientHost/Unicorn.Core.Development.ClientHost.csproj"
COPY . .
WORKDIR "/src/core/development/Unicorn.Core.Development.ClientHost"
RUN dotnet build "Unicorn.Core.Development.ClientHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Core.Development.ClientHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Core.Development.ClientHost.dll"]