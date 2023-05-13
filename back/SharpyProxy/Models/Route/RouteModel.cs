namespace SharpyProxy.Models.Route;

public class RouteModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? MatchPath { get; set; }

    public string[] MatchHosts { get; set; }

    public Guid ClusterId { get; set; }

    public bool Enabled { get; set; }
}