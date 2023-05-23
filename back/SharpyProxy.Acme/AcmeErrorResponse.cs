using System.Text.Json.Serialization;

namespace SharpyProxy.Acme;

public class AcmeErrorResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("detail")]
    public string Detail { get; set; }
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
}