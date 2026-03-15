using System.Net.Http.Json;

namespace PetClinic.Notifications.Api.HttpClients;

public record PetResponse(Guid Id, Guid OwnerId);

public class PetsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PetsApiClient> _logger;

    public PetsApiClient(HttpClient httpClient, ILogger<PetsApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PetResponse?> GetPetAsync(Guid petId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PetResponse>($"/pets/{petId}", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get pet {PetId} from pets API", petId);
            return null;
        }
    }
}
