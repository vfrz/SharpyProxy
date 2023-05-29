namespace SharpyProxy.Acme.Challenge;

public class AcmeChallengeValidationRecord
{
    public required string Url { get; init; }
    
    public required string Hostname { get; init; }
    
    public required string Port { get; init; }
    
    public required string[] AddressesResolved { get; init; }
    
    public required string AddressUsed { get; init; }
}