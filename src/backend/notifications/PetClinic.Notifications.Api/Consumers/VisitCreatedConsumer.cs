using MassTransit;
using PetClinic.Notifications.Api.HttpClients;
using PetClinic.Notifications.Api.Services;
using PetClinic.Notifications.Api.Templates;
using PetClinic.Notifications.Contracts;

namespace PetClinic.Notifications.Api.Consumers;

public class VisitCreatedConsumer : IConsumer<VisitCreated>
{
    private readonly IEmailSender _emailSender;
    private readonly PetsApiClient _petsApiClient;
    private readonly OwnersApiClient _ownersApiClient;
    private readonly ILogger<VisitCreatedConsumer> _logger;

    public VisitCreatedConsumer(
        IEmailSender emailSender,
        PetsApiClient petsApiClient,
        OwnersApiClient ownersApiClient,
        ILogger<VisitCreatedConsumer> logger)
    {
        _emailSender = emailSender;
        _petsApiClient = petsApiClient;
        _ownersApiClient = ownersApiClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<VisitCreated> context)
    {
        var message = context.Message;

        var (owner, petName, ownerName) = await ResolveOwnerAsync(message.PetId, context.CancellationToken);
        if (owner?.Email is null)
        {
            _logger.LogWarning(
                "Skipping confirmation email for visit {VisitId}: owner email not found (PetId={PetId})",
                message.VisitId, message.PetId);
            return;
        }

        var htmlBody = EmailTemplates.VisitConfirmation(
            ownerName: ownerName,
            petName: petName,
            vetName: "your veterinarian",
            scheduledAt: message.ScheduledAt,
            reason: message.Reason);

        await _emailSender.SendAsync(
            toAddress: owner.Email,
            toName: ownerName,
            subject: $"Visit Confirmation — {message.ScheduledAt:MMM d, yyyy} at {message.ScheduledAt:h:mm tt}",
            htmlBody: htmlBody,
            cancellationToken: context.CancellationToken);

        _logger.LogInformation(
            "Sent visit confirmation email to {Email} for visit {VisitId}",
            owner.Email, message.VisitId);

        var reminderTime = message.ScheduledAt.AddHours(-1);
        if (reminderTime > DateTime.UtcNow)
        {
            await context.SchedulePublish(reminderTime, new VisitReminderDue(
                message.VisitId,
                message.PetId,
                message.ScheduledAt,
                message.Reason));

            _logger.LogInformation(
                "Scheduled reminder for visit {VisitId} at {ReminderTime}",
                message.VisitId, reminderTime);
        }
        else
        {
            _logger.LogInformation(
                "Skipping reminder for visit {VisitId}: scheduled time is less than 1 hour away",
                message.VisitId);
        }
    }

    private async Task<(OwnerResponse? Owner, string PetName, string OwnerName)> ResolveOwnerAsync(
        Guid petId,
        CancellationToken cancellationToken)
    {
        var pet = await _petsApiClient.GetPetAsync(petId, cancellationToken);
        if (pet is null)
            return (null, "your pet", "Pet Owner");

        var owner = await _ownersApiClient.GetOwnerAsync(pet.OwnerId, cancellationToken);
        if (owner is null)
            return (null, "your pet", "Pet Owner");

        return (owner, "your pet", $"{owner.FirstName} {owner.LastName}");
    }
}
