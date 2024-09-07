#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["eShop/catalog/Unicorn.eShop.Catalog/Unicorn.eShop.Catalog.csproj", "eShop/catalog/Unicorn.eShop.Catalog/"]
RUN dotnet restore "eShop/catalog/Unicorn.eShop.Catalog/Unicorn.eShop.Catalog.csproj"
COPY . .
WORKDIR "/src/eShop/catalog/Unicorn.eShop.Catalog"
RUN dotnet build "Unicorn.eShop.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.eShop.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.eShop.Catalog.dll"]