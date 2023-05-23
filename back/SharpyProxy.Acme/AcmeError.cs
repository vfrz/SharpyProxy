using System.Text.Json.Serialization;

namespace SharpyProxy.Acme;

public class AcmeError
{
   [JsonPropertyName("type")]
   public string Type { get; set; }
   
   [JsonPropertyName("title")]
   public string Title { get; set; }
   
   [JsonPropertyName("status")]
   public string Status { get; set; }
   
   [JsonPropertyName("detail")]
   public string Detail { get; set; }
   
   [JsonPropertyName("instance")]
   public string Instance { get; set; }
}