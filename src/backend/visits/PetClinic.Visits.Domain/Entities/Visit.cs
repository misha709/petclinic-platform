using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Domain.Entities;

public class Visit
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public Guid VetId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public VisitStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
