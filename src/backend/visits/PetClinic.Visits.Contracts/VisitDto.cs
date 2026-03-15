namespace PetClinic.Visits.Contracts;

public record VisitDto(
    Guid Id,
    Guid PetId,
    Guid VetId,
    DateTime ScheduledAt,
    int DurationMinutes,
    string Reason,
    string? Notes,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
