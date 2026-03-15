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

        var rabbitMqHost = configuration["RabbitMq:Host"] ?? "localhost";
        var rabbitMqUser = configuration["RabbitMq:Username"] ?? "guest";
        var rabbitMqPass = configuration["RabbitMq:Password"] ?? "guest";

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPass);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
