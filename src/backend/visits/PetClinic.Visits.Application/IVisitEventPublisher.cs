namespace PetClinic.Visits.Application;

public interface IVisitEventPublisher
{
    Task PublishVisitCreatedAsync(
        Guid visitId,
        Guid petId,
        Guid vetId,
        DateTime scheduledAt,
        string reason,
        CancellationToken cancellationToken = default);
}
