using System.Security.Cryptography;

namespace SharpyProxy.Acme.Account;

public class AcmeAccount
{
    public required RSAParameters RSAParameters { get; init; }
    
    public required string Url { get; init; }

    public RSA CreateKeyFromParameters() => RSA.Create(RSAParameters);
}