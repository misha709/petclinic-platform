using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetClinic.Visits.Application;
using PetClinic.Visits.Infrastructure.Messaging;
using PetClinic.Visits.Infrastructure.Persistence;

namespace PetClinic.Visits.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVisitsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VisitsDb")
            ?? throw new InvalidOperationException("Connection string 'VisitsDb' not found.");

        services.AddDbContext<VisitsDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IVisitEventPublisher, MassTransitVisitEventPublisher>();

        services.AddMassTransit(x =>
        {
            x.UsingAmazonSqs((ctx, cfg) =>
            {
                cfg.Host("eu-west-1", h =>
                {
                    //TODO 
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
