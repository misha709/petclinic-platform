namespace PetClinic.Notifications.Contracts;

public record VisitReminderDue(
    Guid VisitId,
    Guid PetId,
    DateTime ScheduledAt,
    string Reason);
