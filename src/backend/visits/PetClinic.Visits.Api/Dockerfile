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

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY backend/visits/ backend/visits/
COPY backend/notifications/ backend/notifications/

WORKDIR /src/backend/visits/PetClinic.Visits.Api
RUN dotnet build "PetClinic.Visits.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Visits.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN mkdir -p /migrations && \
    dotnet ef migrations script \
      --idempotent \
      --no-build \
      --output /migrations/migrate.sql \
      --project /src/backend/visits/PetClinic.Visits.Infrastructure \
      --startup-project /src/backend/visits/PetClinic.Visits.Api

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Visits.Api.dll"]

# Migration image — built & pushed separately as petclinic/visits-migrate:<tag>.
# Kubernetes PreSync Job runs: psql -v ON_ERROR_STOP=1 -f /migrations/migrate.sql
# psql reads the connection URI from the PGURI environment variable.
FROM postgres:16-alpine AS migrate
COPY --from=publish /migrations/migrate.sql /migrations/migrate.sql
