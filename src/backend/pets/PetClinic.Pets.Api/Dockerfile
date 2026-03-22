# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj", "backend/pets/PetClinic.Pets.Api/"]
COPY ["backend/pets/PetClinic.Pets.Application/PetClinic.Pets.Application.csproj", "backend/pets/PetClinic.Pets.Application/"]
COPY ["backend/pets/PetClinic.Pets.Contracts/PetClinic.Pets.Contracts.csproj", "backend/pets/PetClinic.Pets.Contracts/"]
COPY ["backend/pets/PetClinic.Pets.Domain/PetClinic.Pets.Domain.csproj", "backend/pets/PetClinic.Pets.Domain/"]
COPY ["backend/pets/PetClinic.Pets.Infrastructure/PetClinic.Pets.Infrastructure.csproj", "backend/pets/PetClinic.Pets.Infrastructure/"]

RUN dotnet restore "backend/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj"

COPY backend/pets/ backend/pets/

WORKDIR /src/backend/pets/PetClinic.Pets.Api
RUN dotnet build "PetClinic.Pets.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Pets.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Pets.Api.dll"]
