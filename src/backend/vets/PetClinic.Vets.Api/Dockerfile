# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["backend/vets/PetClinic.Vets.Api/PetClinic.Vets.Api.csproj", "backend/vets/PetClinic.Vets.Api/"]
COPY ["backend/vets/PetClinic.Vets.Application/PetClinic.Vets.Application.csproj", "backend/vets/PetClinic.Vets.Application/"]
COPY ["backend/vets/PetClinic.Vets.Contracts/PetClinic.Vets.Contracts.csproj", "backend/vets/PetClinic.Vets.Contracts/"]
COPY ["backend/vets/PetClinic.Vets.Domain/PetClinic.Vets.Domain.csproj", "backend/vets/PetClinic.Vets.Domain/"]
COPY ["backend/vets/PetClinic.Vets.Infrastructure/PetClinic.Vets.Infrastructure.csproj", "backend/vets/PetClinic.Vets.Infrastructure/"]

RUN dotnet restore "backend/vets/PetClinic.Vets.Api/PetClinic.Vets.Api.csproj"

COPY backend/vets/ backend/vets/

WORKDIR /src/backend/vets/PetClinic.Vets.Api
RUN dotnet build "PetClinic.Vets.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PetClinic.Vets.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetClinic.Vets.Api.dll"]
