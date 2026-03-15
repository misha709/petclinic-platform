namespace PetClinic.Notifications.Api.Services;

public interface IEmailSender
{
    Task SendAsync(string toAddress, string toName, string subject, string htmlBody, CancellationToken cancellationToken = default);
}
