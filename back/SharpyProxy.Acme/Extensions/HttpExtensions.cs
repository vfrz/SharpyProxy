using System.Net.Http.Json;
using System.Text;

namespace SharpyProxy.Acme.Extensions;

internal static class HttpExtensions
{
    internal static async Task<T> ReadJsonAsync<T>(this HttpContent content)
    {
        return (await content.ReadFromJsonAsync<T>()) ?? throw new Exception("Failed to parse json from http response");
    }

    internal static async Task<string> ToDebugStringAsync(this HttpResponseMessage response)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"[{response.StatusCode}] {response.ReasonPhrase}");
        builder.AppendLine("===Headers===");
        foreach (var header in response.Headers)
            builder.AppendLine($"{header.Key}={string.Join(", ", header.Value)}");
        builder.AppendLine("===Content headers===");
        foreach (var header in response.Content.Headers)
            builder.AppendLine($"{header.Key}={string.Join(", ", header.Value)}");
        builder.AppendLine("===Content===");
        builder.AppendLine(await response.Content.ReadAsStringAsync());
        
        return builder.ToString();
    }
}