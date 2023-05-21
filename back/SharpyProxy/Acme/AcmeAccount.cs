using System.Security.Cryptography;

namespace SharpyProxy.Acme;

public class AcmeAccount
{
    public RSA Key { get; init; }
    
    public string? Url { get; init; }
}