namespace RSSFeedReader.Api.Services;

public sealed class SubscriptionStore
{
    private readonly object _sync = new();
    private readonly List<string> _subscriptions = [];

    public string[] GetAll()
    {
        lock (_sync)
        {
            return _subscriptions.ToArray();
        }
    }

    public bool TryAdd(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        lock (_sync)
        {
            _subscriptions.Add(url);
        }

        return true;
    }
}
