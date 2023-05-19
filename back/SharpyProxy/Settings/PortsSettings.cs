namespace SharpyProxy.Settings;

public class PortsSettings
{
    public const string Section = "Ports";
    
    public int Http { get; set; } = 80;

    public int Https { get; set; } = 443;

    public int Api { get; set; } = 8181;
}