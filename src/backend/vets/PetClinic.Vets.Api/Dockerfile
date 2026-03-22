# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/vets/PetClinic.Vets.Api/PetClinic.Vets.Api.csproj", "backend/vets/PetClinic.Vets.Api/"]
COPY ["backend/vets/PetClinic.Vets.Application/PetClinic.Vets.Application.csproj", "backend/vets/PetClinic.Vets.Application/"]
COPY ["backend/vets/PetClinic.Vets.Contracts/PetClinic.Vets.Contracts.csproj", "backend/vets/PetClinic.Vets.Contracts/"]
COPY ["backend/vets/PetClinic.Vets.Domain/PetClinic.Vets.Domain.csproj", "backend/vets/PetClinic.Vets.Domain/"]
COPY ["backend/vets/PetClinic.Vets.Infrastructure/PetClinic.Vets.Infrastructure.csproj", "backend/vets/PetClinic.Vets.Infrastructure/"]

RUN dotnet restore "backend/vets/PetClinic.Vets.Api/PetClinic.Vets.Api.csproj"

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY backend/vets/ backend/vets/

WORKDIR /src/backend/vets/PetClinic.Vets.Api
RUN dotnet build "PetClinic.Vets.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Vets.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN mkdir -p /migrations && \
    dotnet ef migrations script \
      --idempotent \
      --no-build \
      --output /migrations/migrate.sql \
      --project /src/backend/vets/PetClinic.Vets.Infrastructure \
      --startup-project /src/backend/vets/PetClinic.Vets.Api

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Vets.Api.dll"]

# Migration image — built & pushed separately as petclinic/vets-migrate:<tag>.
# Kubernetes PreSync Job runs: psql -v ON_ERROR_STOP=1 -f /migrations/migrate.sql
# psql reads the connection URI from the PGURI environment variable.
FROM postgres:16-alpine AS migrate
COPY --from=publish /migrations/migrate.sql /migrations/migrate.sql
