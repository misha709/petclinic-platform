using Microsoft.EntityFrameworkCore;
using PetClinic.Visits.Application;
using PetClinic.Visits.Domain.Entities;
using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Infrastructure.Persistence;

public class VisitRepository : IVisitRepository
{
    private readonly VisitsDbContext _db;

    public VisitRepository(VisitsDbContext db)
    {
        _db = db;
    }

    public async Task<Visit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Visits.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Visit>> SearchAsync(Guid? petId, Guid? vetId, DateOnly? date, VisitStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _db.Visits.AsNoTracking();

        if (petId is not null)
            query = query.Where(v => v.PetId == petId);

        if (vetId is not null)
            query = query.Where(v => v.VetId == vetId);

        if (date is not null)
        {
            var dayStart = date.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var dayEnd = dayStart.AddDays(1);
            query = query.Where(v => v.ScheduledAt >= dayStart && v.ScheduledAt < dayEnd);
        }

        if (status is not null)
            query = query.Where(v => v.Status == status);

        return await query.OrderBy(v => v.ScheduledAt).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Visit>> GetOverlappingAsync(Guid vetId, DateTime start, DateTime end, Guid? excludeId, CancellationToken cancellationToken = default)
    {
        var query = _db.Visits.AsNoTracking()
            .Where(v => v.VetId == vetId && v.Status == VisitStatus.Scheduled);

        if (excludeId is not null)
            query = query.Where(v => v.Id != excludeId);

        return await query
            .Where(v => v.ScheduledAt < end &&
                        v.ScheduledAt.AddMinutes(v.DurationMinutes) > start)
            .ToListAsync(cancellationToken);
    }

    public async Task<Visit> AddAsync(Visit visit, CancellationToken cancellationToken = default)
    {
        _db.Visits.Add(visit);
        await _db.SaveChangesAsync(cancellationToken);
        return visit;
    }

    public async Task<Visit?> UpdateAsync(Visit visit, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Visits.FindAsync([visit.Id], cancellationToken);
        if (existing is null)
            return null;

        existing.ScheduledAt = visit.ScheduledAt;
        existing.DurationMinutes = visit.DurationMinutes;
        existing.Reason = visit.Reason;
        existing.Notes = visit.Notes;
        existing.Status = visit.Status;
        existing.UpdatedAt = visit.UpdatedAt;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _db.Visits
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null, cancellationToken);

        if (visit is null)
            return false;

        visit.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
