# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["services/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj", "services/owners/PetClinic.Owners.Api/"]
COPY ["services/owners/PetClinic.Owners.Application/PetClinic.Owners.Application.csproj", "services/owners/PetClinic.Owners.Application/"]
COPY ["services/owners/PetClinic.Owners.Contracts/PetClinic.Owners.Contracts.csproj", "services/owners/PetClinic.Owners.Contracts/"]
COPY ["services/owners/PetClinic.Owners.Domain/PetClinic.Owners.Domain.csproj", "services/owners/PetClinic.Owners.Domain/"]
COPY ["services/owners/PetClinic.Owners.Infrastructure/PetClinic.Owners.Infrastructure.csproj", "services/owners/PetClinic.Owners.Infrastructure/"]

RUN dotnet restore "services/owners/PetClinic.Owners.Api/PetClinic.Owners.Api.csproj"

COPY services/owners/ services/owners/

WORKDIR /src/services/owners/PetClinic.Owners.Api
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
