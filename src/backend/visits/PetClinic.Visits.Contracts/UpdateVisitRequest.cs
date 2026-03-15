namespace PetClinic.Visits.Contracts;

public record UpdateVisitRequest(
    DateTime ScheduledAt,
    int DurationMinutes,
    string Reason,
    string? Notes);
