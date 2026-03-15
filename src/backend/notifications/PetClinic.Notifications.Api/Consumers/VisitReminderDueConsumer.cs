using MassTransit;
using PetClinic.Notifications.Api.HttpClients;
using PetClinic.Notifications.Api.Services;
using PetClinic.Notifications.Api.Templates;
using PetClinic.Notifications.Contracts;

namespace PetClinic.Notifications.Api.Consumers;

public class VisitReminderDueConsumer : IConsumer<VisitReminderDue>
{
    private readonly IEmailSender _emailSender;
    private readonly PetsApiClient _petsApiClient;
    private readonly OwnersApiClient _ownersApiClient;
    private readonly ILogger<VisitReminderDueConsumer> _logger;

    public VisitReminderDueConsumer(
        IEmailSender emailSender,
        PetsApiClient petsApiClient,
        OwnersApiClient ownersApiClient,
        ILogger<VisitReminderDueConsumer> logger)
    {
        _emailSender = emailSender;
        _petsApiClient = petsApiClient;
        _ownersApiClient = ownersApiClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<VisitReminderDue> context)
    {
        var message = context.Message;

        var pet = await _petsApiClient.GetPetAsync(message.PetId, context.CancellationToken);
        if (pet is null)
        {
            _logger.LogWarning(
                "Skipping reminder email for visit {VisitId}: pet {PetId} not found",
                message.VisitId, message.PetId);
            return;
        }

        var owner = await _ownersApiClient.GetOwnerAsync(pet.OwnerId, context.CancellationToken);
        if (owner?.Email is null)
        {
            _logger.LogWarning(
                "Skipping reminder email for visit {VisitId}: owner email not found (OwnerId={OwnerId})",
                message.VisitId, pet.OwnerId);
            return;
        }

        var ownerName = $"{owner.FirstName} {owner.LastName}";

        var htmlBody = EmailTemplates.VisitReminder(
            ownerName: ownerName,
            petName: "your pet",
            vetName: "your veterinarian",
            scheduledAt: message.ScheduledAt,
            reason: message.Reason);

        await _emailSender.SendAsync(
            toAddress: owner.Email,
            toName: ownerName,
            subject: $"Reminder: Visit in 1 Hour — {message.ScheduledAt:h:mm tt}",
            htmlBody: htmlBody,
            cancellationToken: context.CancellationToken);

        _logger.LogInformation(
            "Sent visit reminder email to {Email} for visit {VisitId}",
            owner.Email, message.VisitId);
    }
}
