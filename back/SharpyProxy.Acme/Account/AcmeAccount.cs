using System.Security.Cryptography;

namespace SharpyProxy.Acme.Account;

public class AcmeAccount
{
    public required RSAParameters RSAParameters { get; init; }

    public required string Url { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required AcmeAccountStatus Status { get; init; }

    public required string[] Contact { get; init; }

    public RSA CreateKeyFromParameters() => RSA.Create(RSAParameters);

    public static AcmeAccountStatus ParseStatus(string status)
    {
        return status switch
        {
            "valid" => AcmeAccountStatus.Valid,
            "deactivated" => AcmeAccountStatus.Deactivated,
            "revoked" => AcmeAccountStatus.Revoked,
            _ => throw new Exception($"Failed to parse account status from value: {status}")
        };
    }
}