using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Order;

public class AuthorizationReadyResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }
    
    [JsonPropertyName("identifier")]
    public AcmeIdentifier Identifier { get; set; }
    
    [JsonPropertyName("challenges")]
    public AuthorizationReadyChallenge[] Challenges { get; set; }
}