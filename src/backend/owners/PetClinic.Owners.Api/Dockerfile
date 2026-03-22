# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj", "backend/owners/PetClinic.Owners.Api/"]
COPY ["backend/owners/PetClinic.Owners.Application/PetClinic.Owners.Application.csproj", "backend/owners/PetClinic.Owners.Application/"]
COPY ["backend/owners/PetClinic.Owners.Contracts/PetClinic.Owners.Contracts.csproj", "backend/owners/PetClinic.Owners.Contracts/"]
COPY ["backend/owners/PetClinic.Owners.Domain/PetClinic.Owners.Domain.csproj", "backend/owners/PetClinic.Owners.Domain/"]
COPY ["backend/owners/PetClinic.Owners.Infrastructure/PetClinic.Owners.Infrastructure.csproj", "backend/owners/PetClinic.Owners.Infrastructure/"]

RUN dotnet restore "backend/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj"

COPY backend/owners/ backend/owners/

WORKDIR /src/backend/owners/PetClinic.Owners.Api
RUN dotnet build "PetClinic.Owners.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Owners.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Owners.Api.dll"]
