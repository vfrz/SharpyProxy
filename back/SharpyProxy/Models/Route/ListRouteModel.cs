namespace SharpyProxy.Models.Route;

public class ListRouteModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? MatchPath { get; set; }

    public string[] MatchHosts { get; set; }

    public string ClusterName { get; set; }
    
    public Guid ClusterId { get; set; }

    public bool Enabled { get; set; }
}