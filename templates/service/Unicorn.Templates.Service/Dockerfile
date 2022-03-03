#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["project-templates/Unicorn.Templates.Service.Host/Unicorn.Templates.Service.Host.csproj", "project-templates/Unicorn.Templates.Service.Host/"]
RUN dotnet restore "project-templates/Unicorn.Templates.Service.Host/Unicorn.Templates.Service.Host.csproj"
COPY . .
WORKDIR "/src/project-templates/Unicorn.Templates.Service.Host"
RUN dotnet build "Unicorn.Templates.Service.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Templates.Service.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Templates.Service.Host.dll"]