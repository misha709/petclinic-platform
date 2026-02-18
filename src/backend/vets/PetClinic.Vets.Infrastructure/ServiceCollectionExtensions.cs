using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetClinic.Vets.Application;
using PetClinic.Vets.Infrastructure.Persistence;

namespace PetClinic.Vets.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVetsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VetsDb")
            ?? throw new InvalidOperationException("Connection string 'VetsDb' not found.");

        services.AddDbContext<VetsDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IVetRepository, VetRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();

        return services;
    }
}
