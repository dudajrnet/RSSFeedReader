using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionsApiTests
{
    [Fact]
    public async Task GetSubscriptions_ReturnsEmptyArray()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var subscriptions = await response.Content.ReadFromJsonAsync<string[]>();

        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task PostSubscription_AddsAndListsSubmittedValue()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        const string url = "https://example.com/feed.xml";

        var addResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url });
        var listResponse = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.NoContent, addResponse.StatusCode);
        Assert.Equal(new[] { url }, await listResponse.Content.ReadFromJsonAsync<string[]>());
    }

    [Fact]
    public async Task PostNonUrlValue_PreservesValueWithoutValidation()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        const string url = "not a feed URL";

        var addResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url });
        var listResponse = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.NoContent, addResponse.StatusCode);
        Assert.Equal(new[] { url }, await listResponse.Content.ReadFromJsonAsync<string[]>());
    }

    [Fact]
    public async Task PostDuplicateSubscription_AddsSeparateEntries()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        const string url = "https://example.com/feed.xml";

        var firstResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url });
        var secondResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url });
        var listResponse = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.NoContent, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, secondResponse.StatusCode);
        Assert.Equal(new[] { url, url }, await listResponse.Content.ReadFromJsonAsync<string[]>());
    }

    [Theory]
    [InlineData("{\"url\":\"   \"}")]
    [InlineData("{}")]
    public async Task PostBlankOrMissingUrl_ReturnsBadRequestWithoutAdding(string requestBody)
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        using var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

        var addResponse = await client.PostAsync("/api/subscriptions", content);
        var listResponse = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.BadRequest, addResponse.StatusCode);
        Assert.Equal(Array.Empty<string>(), await listResponse.Content.ReadFromJsonAsync<string[]>());
    }
}
