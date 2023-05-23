using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Account;

internal class AcmeAccountDto
{
    [JsonPropertyName("key")]
    public JsonObject Key { get; set; }
    
    [JsonPropertyName("contact")]
    public string[] Contact { get; set; }
    
    [JsonPropertyName("initialIp")]
    public string InitialIp { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
}