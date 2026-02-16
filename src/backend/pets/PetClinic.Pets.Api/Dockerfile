# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["services/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj", "services/pets/PetClinic.Pets.Api/"]
COPY ["services/pets/PetClinic.Pets.Application/PetClinic.Pets.Application.csproj", "services/pets/PetClinic.Pets.Application/"]
COPY ["services/pets/PetClinic.Pets.Contracts/PetClinic.Pets.Contracts.csproj", "services/pets/PetClinic.Pets.Contracts/"]
COPY ["services/pets/PetClinic.Pets.Domain/PetClinic.Pets.Domain.csproj", "services/pets/PetClinic.Pets.Domain/"]
COPY ["services/pets/PetClinic.Pets.Infrastructure/PetClinic.Pets.Infrastructure.csproj", "services/pets/PetClinic.Pets.Infrastructure/"]

RUN dotnet restore "services/pets/PetClinic.Pets.Api/PetClinic.Pets.Api.csproj"

COPY services/pets/ services/pets/

WORKDIR /src/services/pets/PetClinic.Pets.Api
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
