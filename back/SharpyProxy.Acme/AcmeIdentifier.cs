using System.Text.Json.Serialization;

namespace SharpyProxy.Acme;

public class AcmeIdentifier
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}