namespace SharpyProxy.Services;

public class DomainVerificationService
{
    private readonly Dictionary<string, string> _keys;

    public DomainVerificationService()
    {
        _keys = new Dictionary<string, string>();
    }

    public string GenerateKeyForDomain(string domain)
    {
        var key = Guid.NewGuid().ToString("N");
        _keys[domain] = key;
        return key;
    }

    public string? GetKeyForDomain(string domain)
    {
        return _keys.TryGetValue(domain, out var value) ? value : null;
    }

    public void RemoveKeyForDomain(string domain)
    {
        _keys.Remove(domain);
    }
}