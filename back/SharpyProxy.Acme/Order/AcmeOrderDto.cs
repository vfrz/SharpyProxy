using System.Text.Json.Serialization;

namespace SharpyProxy.Acme.Order;

public class AcmeOrderDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("expires")]
    public DateTime? Expires { get; set; }

    [JsonPropertyName("notBefore")]
    public DateTime? NotBefore { get; set; }

    [JsonPropertyName("notAfter")]
    public DateTime? NotAfter { get; set; }
    
    [JsonPropertyName("error")]
    public AcmeError? Error { get; set; }

    [JsonPropertyName("identifiers")]
    public AcmeIdentifier[] Identifiers { get; set; }

    [JsonPropertyName("authorizations")]
    public string[] Authorizations { get; set; }

    [JsonPropertyName("finalize")]
    public string Finalize { get; set; }

    [JsonPropertyName("certificate")]
    public string? Certificate { get; set; }
}