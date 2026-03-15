namespace PetClinic.Notifications.Contracts;

public record VisitCreated(
    Guid VisitId,
    Guid PetId,
    Guid VetId,
    DateTime ScheduledAt,
    string Reason);
