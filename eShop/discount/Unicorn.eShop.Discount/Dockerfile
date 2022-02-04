#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eShop/discount/Unicorn.eShop.Discount/Unicorn.eShop.Discount.csproj", "eShop/discount/Unicorn.eShop.Discount/"]
RUN dotnet restore "eShop/discount/Unicorn.eShop.Discount/Unicorn.eShop.Discount.csproj"
COPY . .
WORKDIR "/src/eShop/discount/Unicorn.eShop.Discount"
RUN dotnet build "Unicorn.eShop.Discount.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.eShop.Discount.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.eShop.Discount.dll"]