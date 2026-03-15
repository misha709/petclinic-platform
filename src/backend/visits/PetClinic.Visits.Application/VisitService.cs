using PetClinic.Visits.Contracts;
using PetClinic.Visits.Domain.Entities;
using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Application;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _repository;

    public VisitService(IVisitRepository repository)
    {
        _repository = repository;
    }

    public async Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _repository.GetByIdAsync(id, cancellationToken);
        return visit is null ? null : MapToDto(visit);
    }

    public async Task<IReadOnlyList<VisitDto>> SearchAsync(Guid? petId, Guid? vetId, DateOnly? date, VisitStatus? status, CancellationToken cancellationToken = default)
    {
        var visits = await _repository.SearchAsync(petId, vetId, date, status, cancellationToken);
        return visits.Select(MapToDto).ToList();
    }

    public async Task<VisitDto> CreateAsync(CreateVisitRequest request, CancellationToken cancellationToken = default)
    {
        if (request.DurationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(request));

        var end = request.ScheduledAt.AddMinutes(request.DurationMinutes);
        var conflicts = await _repository.GetOverlappingAsync(request.VetId, request.ScheduledAt, end, null, cancellationToken);

        if (conflicts.Count > 0)
            throw new InvalidOperationException("The vet already has a visit scheduled that overlaps with the requested time slot.");

        var visit = new Visit
        {
            Id = Guid.NewGuid(),
            PetId = request.PetId,
            VetId = request.VetId,
            ScheduledAt = request.ScheduledAt,
            DurationMinutes = request.DurationMinutes,
            Reason = request.Reason,
            Notes = request.Notes,
            Status = VisitStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(visit, cancellationToken);
        return MapToDto(created);
    }

    public async Task<VisitDto?> UpdateAsync(Guid id, UpdateVisitRequest request, CancellationToken cancellationToken = default)
    {
        var visit = await _repository.GetByIdAsync(id, cancellationToken);
        if (visit is null)
            return null;

        if (visit.Status == VisitStatus.Cancelled)
            throw new InvalidOperationException("A cancelled visit cannot be updated.");

        if (request.DurationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(request));

        var end = request.ScheduledAt.AddMinutes(request.DurationMinutes);
        var conflicts = await _repository.GetOverlappingAsync(visit.VetId, request.ScheduledAt, end, id, cancellationToken);

        if (conflicts.Count > 0)
            throw new InvalidOperationException("The vet already has a visit scheduled that overlaps with the requested time slot.");

        visit.ScheduledAt = request.ScheduledAt;
        visit.DurationMinutes = request.DurationMinutes;
        visit.Reason = request.Reason;
        visit.Notes = request.Notes;
        visit.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(visit, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<VisitDto?> CancelAsync(Guid id, CancelVisitRequest request, CancellationToken cancellationToken = default)
    {
        var visit = await _repository.GetByIdAsync(id, cancellationToken);
        if (visit is null)
            return null;

        if (visit.Status == VisitStatus.Cancelled)
            throw new InvalidOperationException("The visit is already cancelled.");

        if (visit.Status == VisitStatus.Completed)
            throw new InvalidOperationException("A completed visit cannot be cancelled.");

        visit.Status = VisitStatus.Cancelled;
        visit.Notes = request.Notes ?? visit.Notes;
        visit.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(visit, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<VisitDto?> CompleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _repository.GetByIdAsync(id, cancellationToken);
        if (visit is null)
            return null;

        if (visit.Status == VisitStatus.Completed)
            throw new InvalidOperationException("The visit is already completed.");

        if (visit.Status == VisitStatus.Cancelled)
            throw new InvalidOperationException("A cancelled visit cannot be completed.");

        visit.Status = VisitStatus.Completed;
        visit.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(visit, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    private static VisitDto MapToDto(Visit visit) =>
        new(
            visit.Id,
            visit.PetId,
            visit.VetId,
            visit.ScheduledAt,
            visit.DurationMinutes,
            visit.Reason,
            visit.Notes,
            visit.Status.ToString(),
            visit.CreatedAt,
            visit.UpdatedAt);
}
