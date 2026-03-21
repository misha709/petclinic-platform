namespace PetClinic.Notifications.Api.Messaging;

public sealed class AwsMessagingOptions
{
    public string Region { get; set; } = "eu-west-1";
    public string? AccessKey { get; set; }
    public string? SecretKey { get; set; }
    public string? ServiceUrl { get; set; }
}
