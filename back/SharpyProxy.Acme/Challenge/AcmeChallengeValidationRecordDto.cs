using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Challenge;

public class AcmeChallengeValidationRecordDto
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }
    
    [JsonPropertyName("port")]
    public string Port { get; set; }
    
    [JsonPropertyName("addressesResolved")]
    public string[] AddressesResolved { get; set; }
    
    [JsonPropertyName("addressUsed")]
    public string AddressUsed { get; set; }
}