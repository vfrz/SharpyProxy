using System.Text.Json.Serialization;

namespace SharpyProxy.Acme;

public class AcmeDirectory
{
    [JsonPropertyName("keyChange")]
    public string KeyChange { get; set; }

    [JsonPropertyName("meta")]
    public AcmeDirectoryMeta Meta { get; set; }

    [JsonPropertyName("newAccount")]
    public string NewAccount { get; set; }

    [JsonPropertyName("newNonce")]
    public string NewNonce { get; set; }

    [JsonPropertyName("newOrder")]
    public string NewOrder { get; set; }

    [JsonPropertyName("renewalInfo")]
    public string RenewalInfo { get; set; }

    [JsonPropertyName("revokeCert")]
    public string RevokeCert { get; set; }
}