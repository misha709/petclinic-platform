using Amazon.SQS;
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
                var region = configuration["Aws:Region"] ?? "eu-west-1";
                var serviceUrl = configuration["Aws:ServiceUrl"];
                var accessKey = configuration["Aws:AccessKey"] ?? "dummy";
                var secretKey = configuration["Aws:SecretKey"] ?? "dummy";

                cfg.Host(region, h =>
                {
                    h.AccessKey(accessKey);
                    h.SecretKey(secretKey);
                    if (!string.IsNullOrEmpty(serviceUrl))
                        h.Config(new AmazonSQSConfig { ServiceURL = serviceUrl });
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
