#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["templates/service/Unicorn.Templates.Service/Unicorn.Templates.Service.csproj", "templates/service/Unicorn.Templates.Service/"]
COPY ["templates/service/Unicorn.Templates.Service.SDK/Unicorn.Templates.Service.SDK.csproj", "templates/service/Unicorn.Templates.Service.SDK/"]
RUN dotnet restore "templates/service/Unicorn.Templates.Service/Unicorn.Templates.Service.csproj"
COPY . .
WORKDIR "/src/templates/service/Unicorn.Templates.Service"
RUN dotnet build "Unicorn.Templates.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Templates.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Templates.Service.dll"]