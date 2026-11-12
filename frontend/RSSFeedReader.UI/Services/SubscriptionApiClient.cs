using System.Net.Http.Json;
using System.Text.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<string>> GetSubscriptionsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var subscriptions = await httpClient.GetFromJsonAsync<string[]>(
                "subscriptions",
                cancellationToken);

            return subscriptions
                ?? throw new InvalidDataException("The subscriptions API returned an empty response.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("The subscriptions API returned invalid JSON.", exception);
        }
    }

    public async Task AddSubscriptionAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "subscriptions",
            new { url },
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
