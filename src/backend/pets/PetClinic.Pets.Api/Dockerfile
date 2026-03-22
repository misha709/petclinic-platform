# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj", "backend/pets/PetClinic.Pets.Api/"]
COPY ["backend/pets/PetClinic.Pets.Application/PetClinic.Pets.Application.csproj", "backend/pets/PetClinic.Pets.Application/"]
COPY ["backend/pets/PetClinic.Pets.Contracts/PetClinic.Pets.Contracts.csproj", "backend/pets/PetClinic.Pets.Contracts/"]
COPY ["backend/pets/PetClinic.Pets.Domain/PetClinic.Pets.Domain.csproj", "backend/pets/PetClinic.Pets.Domain/"]
COPY ["backend/pets/PetClinic.Pets.Infrastructure/PetClinic.Pets.Infrastructure.csproj", "backend/pets/PetClinic.Pets.Infrastructure/"]

RUN dotnet restore "backend/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj"

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY backend/pets/ backend/pets/

WORKDIR /src/backend/pets/PetClinic.Pets.Api
RUN dotnet build "PetClinic.Pets.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Pets.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN mkdir -p /migrations && \
    dotnet ef migrations script \
      --idempotent \
      --no-build \
      --output /migrations/migrate.sql \
      --project /src/backend/pets/PetClinic.Pets.Infrastructure \
      --startup-project /src/backend/pets/PetClinic.Pets.Api

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Pets.Api.dll"]

# Migration image — built & pushed separately as petclinic/pets-migrate:<tag>.
# Kubernetes PreSync Job runs: psql -v ON_ERROR_STOP=1 -f /migrations/migrate.sql
# psql reads the connection URI from the PGURI environment variable.
FROM postgres:16-alpine AS migrate
COPY --from=publish /migrations/migrate.sql /migrations/migrate.sql
