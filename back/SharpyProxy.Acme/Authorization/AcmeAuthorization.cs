namespace SharpyProxy.Acme.Authorization;

public class AcmeAuthorization
{
    public required string Url { get; init; }

    public required AcmeIdentifier Identifier { get; init; }

    public required AcmeAuthorizationStatus Status { get; init; }

    public required DateTime? Expires { get; init; }

    public required AcmeAuthorizationChallenge[] Challenges { get; init; }

    public required bool? Wildcard { get; init; }

    public AcmeAuthorizationChallenge? HttpChallenge => Challenges.SingleOrDefault(c => c.Type is AcmeAuthorizationChallengeType.Http01);

    public AcmeAuthorizationChallenge? DnsChallenge => Challenges.SingleOrDefault(c => c.Type is AcmeAuthorizationChallengeType.Dns01);

    public static AcmeAuthorizationStatus ParseStatus(string status)
    {
        return status switch
        {
            "pending" => AcmeAuthorizationStatus.Pending,
            "valid" => AcmeAuthorizationStatus.Valid,
            "invalid" => AcmeAuthorizationStatus.Invalid,
            "deactivated" => AcmeAuthorizationStatus.Deactivated,
            "expired" => AcmeAuthorizationStatus.Expired,
            "revoked" => AcmeAuthorizationStatus.Revoked,
            _ => throw new Exception($"Failed to parse authorization status from value: {status}")
        };
    }
}