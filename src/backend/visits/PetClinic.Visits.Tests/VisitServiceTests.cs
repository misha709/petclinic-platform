using FluentAssertions;
using NSubstitute;
using PetClinic.Visits.Application;
using PetClinic.Visits.Contracts;
using PetClinic.Visits.Domain.Entities;
using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Tests;

public class VisitServiceTests
{
    private readonly IVisitRepository _repository = Substitute.For<IVisitRepository>();
    private readonly IVisitEventPublisher _eventPublisher = Substitute.For<IVisitEventPublisher>();
    private readonly VisitService _sut;

    public VisitServiceTests()
    {
        _sut = new VisitService(_repository, _eventPublisher);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task CreateAsync_Throws_WhenDurationIsNotPositive(int duration)
    {
        var request = new CreateVisitRequest(
            PetId: Guid.NewGuid(),
            VetId: Guid.NewGuid(),
            ScheduledAt: DateTime.UtcNow.AddDays(1),
            DurationMinutes: duration,
            Reason: "Checkup",
            Notes: null);

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Duration*greater than 0*");
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenVetHasConflictingVisit()
    {
        _repository.GetOverlappingAsync(
                Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
                Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(new List<Visit> { new() });

        var request = new CreateVisitRequest(
            PetId: Guid.NewGuid(),
            VetId: Guid.NewGuid(),
            ScheduledAt: DateTime.UtcNow.AddDays(1),
            DurationMinutes: 30,
            Reason: "Checkup",
            Notes: null);

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*overlaps*");
    }

    [Fact]
    public async Task CreateAsync_PublishesEvent_AndReturnsScheduledVisit()
    {
        _repository.GetOverlappingAsync(
                Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
                Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Visit>());
        _repository.AddAsync(Arg.Any<Visit>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Visit>());

        var request = new CreateVisitRequest(
            PetId: Guid.NewGuid(),
            VetId: Guid.NewGuid(),
            ScheduledAt: DateTime.UtcNow.AddDays(1),
            DurationMinutes: 30,
            Reason: "Annual checkup",
            Notes: null);

        var result = await _sut.CreateAsync(request);

        result.Id.Should().NotBeEmpty();
        result.Status.Should().Be(nameof(VisitStatus.Scheduled));
        await _eventPublisher.Received(1).PublishVisitCreatedAsync(
            Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(),
            Arg.Any<DateTime>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenVisitIsCancelled()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Visit { Id = id, Status = VisitStatus.Cancelled });

        var act = () => _sut.UpdateAsync(id, new UpdateVisitRequest(
            ScheduledAt: DateTime.UtcNow.AddDays(1),
            DurationMinutes: 30,
            Reason: "Checkup",
            Notes: null));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*cancelled*");
    }

    [Fact]
    public async Task CancelAsync_Throws_WhenVisitIsAlreadyCancelled()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Visit { Id = id, Status = VisitStatus.Cancelled });

        var act = () => _sut.CancelAsync(id, new CancelVisitRequest(Notes: null));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already cancelled*");
    }

    [Fact]
    public async Task CancelAsync_Throws_WhenVisitIsCompleted()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Visit { Id = id, Status = VisitStatus.Completed });

        var act = () => _sut.CancelAsync(id, new CancelVisitRequest(Notes: null));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*completed*cannot be cancelled*");
    }

    [Fact]
    public async Task CompleteAsync_Throws_WhenVisitIsAlreadyCompleted()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Visit { Id = id, Status = VisitStatus.Completed });

        var act = () => _sut.CompleteAsync(id);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already completed*");
    }

    [Fact]
    public async Task CompleteAsync_Throws_WhenVisitIsCancelled()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Visit { Id = id, Status = VisitStatus.Cancelled });

        var act = () => _sut.CompleteAsync(id);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*cancelled*cannot be completed*");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenVisitNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Visit?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }
}
