using MassTransit;
using PetClinic.Notifications.Contracts;
using PetClinic.Visits.Application;

namespace PetClinic.Visits.Infrastructure.Messaging;

public class MassTransitVisitEventPublisher : IVisitEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitVisitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishVisitCreatedAsync(
        Guid visitId,
        Guid petId,
        Guid vetId,
        DateTime scheduledAt,
        string reason,
        CancellationToken cancellationToken = default)
    {
        return _publishEndpoint.Publish(
            new VisitCreated(visitId, petId, vetId, scheduledAt, reason),
            cancellationToken);
    }
}
