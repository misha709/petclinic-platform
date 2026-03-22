using FluentAssertions;
using PetClinic.Notifications.Api.Templates;

namespace PetClinic.Notifications.Tests;

public class EmailTemplatesTests
{
    [Fact]
    public void VisitConfirmation_ReturnsNonEmptyHtml_ContainingOwnerAndPetName()
    {
        var html = EmailTemplates.VisitConfirmation(
            ownerName: "John Doe",
            petName: "Buddy",
            vetName: "Dr. Smith",
            scheduledAt: new DateTime(2026, 6, 15, 10, 0, 0, DateTimeKind.Utc),
            reason: "Annual checkup");

        html.Should().NotBeNullOrEmpty();
        html.Should().Contain("John Doe");
        html.Should().Contain("Buddy");
        html.Should().Contain("Annual checkup");
    }

    [Fact]
    public void VisitReminder_ReturnsNonEmptyHtml_ContainingOwnerAndReason()
    {
        var html = EmailTemplates.VisitReminder(
            ownerName: "Jane Doe",
            petName: "Whiskers",
            vetName: "Dr. Jones",
            scheduledAt: new DateTime(2026, 6, 15, 14, 30, 0, DateTimeKind.Utc),
            reason: "Vaccination");

        html.Should().NotBeNullOrEmpty();
        html.Should().Contain("Jane Doe");
        html.Should().Contain("Vaccination");
    }
}
