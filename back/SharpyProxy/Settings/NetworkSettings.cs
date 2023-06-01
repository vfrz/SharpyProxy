namespace SharpyProxy.Settings;

public class NetworkSettings
{
    public const string Section = "Network";

    public int ProxyHttpPort { get; set; } = 80;

    public int ProxyHttpsPort { get; set; } = 443;

    public int ApiPort { get; set; } = 8181;

    public string DashboardUrl { get; set; } = "http://localhost:3000";
}