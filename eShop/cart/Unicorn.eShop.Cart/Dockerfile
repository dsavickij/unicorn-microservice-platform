#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eShop/cart/Unicorn.eShop.Cart/Unicorn.eShop.Cart.csproj", "eShop/cart/Unicorn.eShop.Cart/"]
COPY ["eShop/cart/Unicorn.eShop.Cart.DataAccess/Unicorn.eShop.Cart.DataAccess.csproj", "eShop/cart/Unicorn.eShop.Cart.DataAccess/"]
COPY ["eShop/cart/Unicorn.eShop.Cart.SDK/Unicorn.eShop.Cart.SDK.csproj", "eShop/cart/Unicorn.eShop.Cart.SDK/"]
RUN dotnet restore "eShop/cart/Unicorn.eShop.Cart/Unicorn.eShop.Cart.csproj"
COPY . .
WORKDIR "/src/eShop/cart/Unicorn.eShop.Cart"
RUN dotnet build "Unicorn.eShop.Cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.eShop.Cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.eShop.Cart.dll"]