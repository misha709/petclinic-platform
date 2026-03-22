using FluentAssertions;
using NSubstitute;
using PetClinic.Pets.Application;
using PetClinic.Pets.Contracts;
using PetClinic.Pets.Domain.Entities;
using PetClinic.Pets.Domain.Enums;

namespace PetClinic.Pets.Tests;

public class PetServiceTests
{
    private readonly IPetRepository _repository = Substitute.For<IPetRepository>();
    private readonly PetService _sut;

    public PetServiceTests()
    {
        _sut = new PetService(_repository);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPetNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Pet?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenPetTypeIsInvalid()
    {
        var request = new CreatePetRequest(
            Name: "Buddy",
            PetType: "Dinosaur",
            Breed: "Unknown",
            BirthDate: null,
            OwnerId: Guid.NewGuid());

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Dinosaur*");
    }

    [Fact]
    public async Task CreateAsync_ReturnsMappedDto_WithCorrectPetType()
    {
        var validType = Enum.GetNames<PetType>()[0];
        var ownerId = Guid.NewGuid();
        var request = new CreatePetRequest(
            Name: "Max",
            PetType: validType,
            Breed: "Labrador",
            BirthDate: new DateTime(2020, 1, 1),
            OwnerId: ownerId);

        _repository.AddAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Pet>());

        var result = await _sut.CreateAsync(request);

        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be("Max");
        result.PetType.Should().Be(validType);
        result.OwnerId.Should().Be(ownerId);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenPetTypeIsInvalid()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Pet { Id = id });

        var request = new UpdatePetRequest(
            Name: "Max",
            PetType: "Dragon",
            Breed: "Unknown",
            BirthDate: null,
            OwnerId: Guid.NewGuid());

        var act = () => _sut.UpdateAsync(id, request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Dragon*");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenPetNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Pet?)null);

        var result = await _sut.UpdateAsync(
            Guid.NewGuid(),
            new UpdatePetRequest("A", "Dog", "B", null, Guid.NewGuid()));

        result.Should().BeNull();
    }
}
