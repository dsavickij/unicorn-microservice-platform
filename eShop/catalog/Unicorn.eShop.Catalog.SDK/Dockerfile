#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eShop/catalog/Unicorn.eShop.Catalog.SDK/Unicorn.eShop.Catalog.SDK.csproj", "eShop/catalog/Unicorn.eShop.Catalog.SDK/"]
RUN dotnet restore "eShop/catalog/Unicorn.eShop.Catalog.SDK/Unicorn.eShop.Catalog.SDK.csproj"
COPY . .
WORKDIR "/src/eShop/catalog/Unicorn.eShop.Catalog.SDK"
RUN dotnet build "Unicorn.eShop.Catalog.SDK.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.eShop.Catalog.SDK.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.eShop.Catalog.SDK.dll"]