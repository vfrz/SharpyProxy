using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Challenge;

public class AcmeChallengeDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("validationRecord")]
    public AcmeChallengeValidationRecordDto[]? ValidationRecord { get; set; }

    [JsonPropertyName("validated")]
    public DateTime? Validated { get; set; }
    
    [JsonPropertyName("error")]
    public AcmeError? Error { get; set; }
}