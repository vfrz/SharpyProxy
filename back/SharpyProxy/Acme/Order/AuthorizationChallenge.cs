using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Order;

public class AuthorizationChallenge
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
}