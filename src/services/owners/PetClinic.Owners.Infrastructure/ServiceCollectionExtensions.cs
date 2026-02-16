using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetClinic.Owners.Application;
using PetClinic.Owners.Infrastructure.Persistence;

namespace PetClinic.Owners.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOwnersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OwnersDb")
            ?? throw new InvalidOperationException("Connection string 'OwnersDb' not found.");

        services.AddDbContext<OwnersDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IOwnerRepository, OwnerRepository>();

        return services;
    }
}
