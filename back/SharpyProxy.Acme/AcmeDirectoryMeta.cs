using System.Text.Json.Serialization;

namespace SharpyProxy.Acme;

public class AcmeDirectoryMeta
{
    [JsonPropertyName("caaIdentities")]
    public string[] CaaIdentities { get; set; }

    [JsonPropertyName("termsOfService")]
    public string TermsOfService { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }
}