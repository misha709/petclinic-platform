# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/visits/PetClinic.Visits.Api/PetClinic.Visits.Api.csproj", "backend/visits/PetClinic.Visits.Api/"]
COPY ["backend/visits/PetClinic.Visits.Application/PetClinic.Visits.Application.csproj", "backend/visits/PetClinic.Visits.Application/"]
COPY ["backend/visits/PetClinic.Visits.Contracts/PetClinic.Visits.Contracts.csproj", "backend/visits/PetClinic.Visits.Contracts/"]
COPY ["backend/visits/PetClinic.Visits.Domain/PetClinic.Visits.Domain.csproj", "backend/visits/PetClinic.Visits.Domain/"]
COPY ["backend/visits/PetClinic.Visits.Infrastructure/PetClinic.Visits.Infrastructure.csproj", "backend/visits/PetClinic.Visits.Infrastructure/"]
COPY ["backend/notifications/PetClinic.Notifications.Contracts/PetClinic.Notifications.Contracts.csproj", "backend/notifications/PetClinic.Notifications.Contracts/"]

RUN dotnet restore "backend/visits/PetClinic.Visits.Api/PetClinic.Visits.Api.csproj"

COPY backend/visits/ backend/visits/
COPY backend/notifications/ backend/notifications/

WORKDIR /src/backend/visits/PetClinic.Visits.Api
RUN dotnet build "PetClinic.Visits.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Visits.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Visits.Api.dll"]
