namespace SharpyProxy.Acme.Challenge;

public class AcmeChallenge
{
    public required string Url { get; init; }
    
    public required AcmeChallengeType Type { get; init; }

    public required AcmeChallengeStatus Status { get; init; }

    public required string Token { get; init; }

    public required AcmeChallengeValidationRecord[] ValidationRecord { get; init; }

    public required DateTime? Validated { get; init; }
    
    public required AcmeError? Error { get; init; }

    public static AcmeChallengeType ParseType(string type)
    {
        return type switch
        {
            "http-01" => AcmeChallengeType.Http01,
            "dns-01" => AcmeChallengeType.Dns01,
            "tls-alpn-01" => AcmeChallengeType.TlsAlpn01,
            "tls-sni-01" => AcmeChallengeType.TlsSni01,
            _ => throw new Exception($"Failed to parse challenge type from value: {type}")
        };
    }
    
    public static AcmeChallengeStatus ParseStatus(string status)
    {
        return status switch
        {
            "pending" => AcmeChallengeStatus.Pending,
            "processing" => AcmeChallengeStatus.Processing,
            "valid" => AcmeChallengeStatus.Valid,
            "invalid" => AcmeChallengeStatus.Invalid,
            _ => throw new Exception($"Failed to parse challenge status from value: {status}")
        };
    }
}