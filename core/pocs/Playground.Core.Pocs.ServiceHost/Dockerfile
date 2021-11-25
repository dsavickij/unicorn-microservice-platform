#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Playground.Core.ProofOfConcepts.TestingHost/Playground.Core.ProofOfConcepts.TestingHost.csproj", "Playground.Core.ProofOfConcepts.TestingHost/"]
RUN dotnet restore "Playground.Core.ProofOfConcepts.TestingHost/Playground.Core.ProofOfConcepts.TestingHost.csproj"
COPY . .
WORKDIR "/src/Playground.Core.ProofOfConcepts.TestingHost"
RUN dotnet build "Playground.Core.ProofOfConcepts.TestingHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Playground.Core.ProofOfConcepts.TestingHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Playground.Core.ProofOfConcepts.TestingHost.dll"]