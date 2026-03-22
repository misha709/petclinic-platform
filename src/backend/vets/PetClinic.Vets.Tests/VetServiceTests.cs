using FluentAssertions;
using NSubstitute;
using PetClinic.Vets.Application;
using PetClinic.Vets.Contracts;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Tests;

public class VetServiceTests
{
    private readonly IVetRepository _repository = Substitute.For<IVetRepository>();
    private readonly VetService _sut;

    public VetServiceTests()
    {
        _sut = new VetService(_repository);
    }

    [Fact]
    public async Task GetByIdAsync_UsesGetWithSpecializations()
    {
        var id = Guid.NewGuid();
        _repository.GetVetWithSpecializationsAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Vet { Id = id, FirstName = "Alice", LastName = "Smith", CreatedAt = DateTime.UtcNow });

        await _sut.GetByIdAsync(id);

        await _repository.Received(1).GetVetWithSpecializationsAsync(id, Arg.Any<CancellationToken>());
        await _repository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenVetNotFound()
    {
        _repository.GetVetWithSpecializationsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Vet?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ReturnsDto_WithNewId()
    {
        var request = new CreateVetRequest(FirstName: "Bob", LastName: "Jones");
        _repository.AddAsync(Arg.Any<Vet>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Vet>());

        var result = await _sut.CreateAsync(request);

        result.Id.Should().NotBeEmpty();
        result.FirstName.Should().Be("Bob");
        result.LastName.Should().Be("Jones");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenVetNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Vet?)null);

        var result = await _sut.UpdateAsync(Guid.NewGuid(), new UpdateVetRequest("A", "B"));

        result.Should().BeNull();
    }

    [Fact]
    public async Task AssignSpecializationsAsync_ReturnsNull_WhenVetNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Vet?)null);

        var result = await _sut.AssignSpecializationsAsync(
            Guid.NewGuid(),
            new AssignSpecializationsRequest(SpecializationIds: [1, 2]));

        result.Should().BeNull();
    }
}

public class SpecializationServiceTests
{
    private readonly ISpecializationRepository _repository = Substitute.For<ISpecializationRepository>();
    private readonly SpecializationService _sut;

    public SpecializationServiceTests()
    {
        _sut = new SpecializationService(_repository);
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenNameAlreadyExists()
    {
        _repository.GetByNameAsync("Surgery", Arg.Any<CancellationToken>())
            .Returns(new Specialization { Id = 1, Name = "Surgery" });

        var act = () => _sut.CreateAsync(new CreateSpecializationRequest(Name: "Surgery"));

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Surgery*already exists*");
    }

    [Fact]
    public async Task CreateAsync_ReturnsDto_WhenNameIsUnique()
    {
        _repository.GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Specialization?)null);
        _repository.AddAsync(Arg.Any<Specialization>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Specialization>());

        var result = await _sut.CreateAsync(new CreateSpecializationRequest(Name: "Dentistry"));

        result.Name.Should().Be("Dentistry");
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenNewNameConflictsWithDifferentEntry()
    {
        var targetId = 1;
        var conflictingId = 2;
        _repository.GetByIdAsync(targetId, Arg.Any<CancellationToken>())
            .Returns(new Specialization { Id = targetId, Name = "Surgery" });
        _repository.GetByNameAsync("Radiology", Arg.Any<CancellationToken>())
            .Returns(new Specialization { Id = conflictingId, Name = "Radiology" });

        var act = () => _sut.UpdateAsync(targetId, new UpdateSpecializationRequest(Name: "Radiology"));

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Radiology*already exists*");
    }
}
