using System.Net.Http.Json;

namespace PetClinic.Notifications.Api.HttpClients;

public record OwnerResponse(Guid Id, string FirstName, string LastName, string? Email);

public class OwnersApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OwnersApiClient> _logger;

    public OwnersApiClient(HttpClient httpClient, ILogger<OwnersApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OwnerResponse?> GetOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<OwnerResponse>($"/owners/{ownerId}", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get owner {OwnerId} from owners API", ownerId);
            return null;
        }
    }
}
