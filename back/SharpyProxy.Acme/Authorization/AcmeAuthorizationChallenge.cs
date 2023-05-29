namespace SharpyProxy.Acme.Authorization;

public class AcmeAuthorizationChallenge
{
    public required AcmeAuthorizationChallengeType Type { get; init; }

    public required AcmeAuthorizationChallengeStatus Status { get; init; }

    public required string Url { get; init; }

    public required string Token { get; init; }

    public required DateTime? Validated { get; init; }

    public required AcmeError? Error { get; init; }

    public static AcmeAuthorizationChallengeType ParseType(string type)
    {
        return type switch
        {
            "http-01" => AcmeAuthorizationChallengeType.Http01,
            "dns-01" => AcmeAuthorizationChallengeType.Dns01,
            "tls-alpn-01" => AcmeAuthorizationChallengeType.TlsAlpn01,
            "tls-sni-01" => AcmeAuthorizationChallengeType.TlsSni01,
            _ => throw new Exception($"Failed to parse challenge type from value: {type}")
        };
    }

    public static AcmeAuthorizationChallengeStatus ParseStatus(string status)
    {
        return status switch
        {
            "pending" => AcmeAuthorizationChallengeStatus.Pending,
            "processing" => AcmeAuthorizationChallengeStatus.Processing,
            "valid" => AcmeAuthorizationChallengeStatus.Valid,
            "invalid" => AcmeAuthorizationChallengeStatus.Invalid,
            _ => throw new Exception($"Failed to parse challenge status from value: {status}")
        };
    }
}