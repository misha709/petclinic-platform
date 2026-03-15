# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/notifications/PetClinic.Notifications.Api/PetClinic.Notifications.Api.csproj", "backend/notifications/PetClinic.Notifications.Api/"]
COPY ["backend/notifications/PetClinic.Notifications.Contracts/PetClinic.Notifications.Contracts.csproj", "backend/notifications/PetClinic.Notifications.Contracts/"]

RUN dotnet restore "backend/notifications/PetClinic.Notifications.Api/PetClinic.Notifications.Api.csproj"

COPY backend/notifications/ backend/notifications/

WORKDIR /src/backend/notifications/PetClinic.Notifications.Api
RUN dotnet build "PetClinic.Notifications.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Notifications.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Notifications.Api.dll"]
