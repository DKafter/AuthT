FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthExample.API/AuthExample.API.csproj", "AuthExample.API/"]
COPY ["AuthExample.Models/AuthExample.Models.csproj", "AuthExample.Models/"]
COPY ["AuthExample.Database/AuthExample.Database.csproj", "AuthExample.Database/"]
RUN dotnet restore "./AuthExample.API/AuthExample.API.csproj"
COPY . .
WORKDIR "/src/AuthExample.API"
RUN dotnet build "./AuthExample.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthExample.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Создаем директорию для данных SQLite
RUN mkdir -p /app/data
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthExample.API.dll"]