using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Order;

public class AuthorizationReadyChallenge
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("validated")]
    public DateTime? Validated { get; set; }
}