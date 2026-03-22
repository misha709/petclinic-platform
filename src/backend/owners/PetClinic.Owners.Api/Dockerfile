# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj", "backend/owners/PetClinic.Owners.Api/"]
COPY ["backend/owners/PetClinic.Owners.Application/PetClinic.Owners.Application.csproj", "backend/owners/PetClinic.Owners.Application/"]
COPY ["backend/owners/PetClinic.Owners.Contracts/PetClinic.Owners.Contracts.csproj", "backend/owners/PetClinic.Owners.Contracts/"]
COPY ["backend/owners/PetClinic.Owners.Domain/PetClinic.Owners.Domain.csproj", "backend/owners/PetClinic.Owners.Domain/"]
COPY ["backend/owners/PetClinic.Owners.Infrastructure/PetClinic.Owners.Infrastructure.csproj", "backend/owners/PetClinic.Owners.Infrastructure/"]

RUN dotnet restore "backend/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj"

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY backend/owners/ backend/owners/

WORKDIR /src/backend/owners/PetClinic.Owners.Api
RUN dotnet build "PetClinic.Owners.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Owners.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN mkdir -p /migrations && \
    dotnet ef migrations script \
      --idempotent \
      --no-build \
      --output /migrations/migrate.sql \
      --project /src/backend/owners/PetClinic.Owners.Infrastructure \
      --startup-project /src/backend/owners/PetClinic.Owners.Api

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Owners.Api.dll"]

# Migration image — built & pushed separately as petclinic/owners-migrate:<tag>.
# Kubernetes PreSync Job runs: psql -v ON_ERROR_STOP=1 -f /migrations/migrate.sql
# psql reads the connection URI from the PGURI environment variable.
FROM postgres:16-alpine AS migrate
COPY --from=publish /migrations/migrate.sql /migrations/migrate.sql
