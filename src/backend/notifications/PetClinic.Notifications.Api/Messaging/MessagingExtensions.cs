using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using Microsoft.Extensions.Options;
using PetClinic.Notifications.Api.Consumers;

namespace PetClinic.Notifications.Api.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsMessagingOptions>(
            configuration.GetSection("AwsMessaging"));

        services.AddMassTransit(x =>
        {
            x.AddQuartzConsumers();
            x.AddConsumer<VisitCreatedConsumer>();
            x.AddConsumer<VisitReminderDueConsumer>();

            x.UsingAmazonSqs((context, cfg) =>
            {
                var opts = context
                    .GetRequiredService<IOptions<AwsMessagingOptions>>().Value;

                cfg.Host(opts.Region, h =>
                {
                    if (!string.IsNullOrWhiteSpace(opts.AccessKey))
                    {
                        h.AccessKey(opts.AccessKey);
                        h.SecretKey(opts.SecretKey!);
                    }

                    if (!string.IsNullOrWhiteSpace(opts.ServiceUrl))
                    {
                        h.Config(new AmazonSQSConfig { ServiceURL = opts.ServiceUrl });
                        h.Config(new AmazonSimpleNotificationServiceConfig
                            { ServiceURL = opts.ServiceUrl });
                    }
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
