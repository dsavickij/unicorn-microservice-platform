#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["core/services/authentication/Unicorn.Core.Services.Authentication.OpenIddict/Unicorn.Core.Services.Authentication.OpenIddict.csproj", "core/services/authentication/Unicorn.Core.Services.Authentication.OpenIddict/"]
RUN dotnet restore "core/services/authentication/Unicorn.Core.Services.Authentication.OpenIddict/Unicorn.Core.Services.Authentication.OpenIddict.csproj"
COPY . .
WORKDIR "/src/core/services/authentication/Unicorn.Core.Services.Authentication.OpenIddict"
RUN dotnet build "Unicorn.Core.Services.Authentication.OpenIddict.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unicorn.Core.Services.Authentication.OpenIddict.csproj" -c Release -o /app/publish
FROM base AS final

#COPY /certificate/mkcert-v1.4.3-linux-amd64 /usr/local/bin
#COPY /certificate/rootCA*.pem /root/.local/share/mkcert/
#RUN chmod +x /usr/local/bin/mkcert \
  #&& mkcert -install \
  #&& rm -rf /usr/local/bin/mkcert 

#COPY mkcert-v1.4.3-linux-amd64 /usr/local/bin
#COPY rootCA*.pem /root/.local/share/mkcert/
#RUN chmod +x /usr/local/bin/mkcert-v1.4.3-linux-amd64 \
  #&& mkcert-v1.4.3-linux-amd64 -install \
  #&& rm -rf /usr/local/bin/mkcert-v1.4.3-linux-amd64
  
#COPY mkcert /usr/local/bin
#COPY rootCA*.pem /root/.local/share/mkcert/
#RUN chmod +x /usr/local/bin/mkcert \
  #&& mkcert -install \
  #&& rm -rf /usr/local/bin/mkcert 

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unicorn.Core.Services.Authentication.OpenIddict.dll"]