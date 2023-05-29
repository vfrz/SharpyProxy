using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Authorization;

internal class AcmeAuthorizationDto
{
    [JsonPropertyName("identifier")]
    public AcmeIdentifier Identifier { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("expires")]
    public DateTime? Expires { get; set; }
    
    [JsonPropertyName("challenges")]
    public AcmeAuthorizationChallengeDto[] Challenges { get; set; }
    
    [JsonPropertyName("wildcard")]
    public bool? Wildcard { get; set; }
}