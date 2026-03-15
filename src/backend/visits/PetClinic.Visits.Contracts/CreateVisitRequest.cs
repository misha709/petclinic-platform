namespace PetClinic.Visits.Contracts;

public record CreateVisitRequest(
    Guid PetId,
    Guid VetId,
    DateTime ScheduledAt,
    int DurationMinutes,
    string Reason,
    string? Notes);
