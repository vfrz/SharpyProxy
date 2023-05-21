namespace SharpyProxy.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> ReadJsonAsync<T>(this HttpContent content)
    {
        return (await content.ReadFromJsonAsync<T>()) ?? throw new Exception("Failed to parse json from http response");
    }
}