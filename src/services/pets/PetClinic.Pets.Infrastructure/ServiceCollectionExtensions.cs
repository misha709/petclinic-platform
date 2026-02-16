using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetClinic.Pets.Application;
using PetClinic.Pets.Infrastructure.Persistence;

namespace PetClinic.Pets.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPetsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PetsDb")
            ?? throw new InvalidOperationException("Connection string 'PetsDb' not found.");

        services.AddDbContext<PetsDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPetRepository, PetRepository>();

        return services;
    }
}
