using FluentAssertions;
using NSubstitute;
using PetClinic.Owners.Application;
using PetClinic.Owners.Contracts;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Tests;

public class OwnerServiceTests
{
    private readonly IOwnerRepository _repository = Substitute.For<IOwnerRepository>();
    private readonly OwnerService _sut;

    public OwnerServiceTests()
    {
        _sut = new OwnerService(_repository);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenOwnerNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Owner?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedDto_WhenOwnerExists()
    {
        var id = Guid.NewGuid();
        var owner = new Owner
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St",
            City = "Springfield",
            Telephone = "555-1234",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow
        };
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(owner);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task CreateAsync_ReturnsDto_WithNewId_AndTrimsEmail()
    {
        var request = new CreateOwnerRequest(
            FirstName: "Jane",
            LastName: "Doe",
            Address: "456 Elm St",
            City: "Shelbyville",
            Telephone: "555-5678",
            Email: "  jane@example.com  ");

        _repository.AddAsync(Arg.Any<Owner>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Owner>());

        var result = await _sut.CreateAsync(request);

        result.Id.Should().NotBeEmpty();
        result.FirstName.Should().Be("Jane");
        result.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenOwnerNotFound()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Owner?)null);

        var result = await _sut.UpdateAsync(
            Guid.NewGuid(),
            new UpdateOwnerRequest("A", "B", "C", "D", "E", "f@g.com"));

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DelegatesToRepository_AndReturnsResult()
    {
        var id = Guid.NewGuid();
        _repository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeTrue();
        await _repository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }
}
