namespace SharpyProxy.Models.Route;

public class RouteModel
{
    public string Id { get; set; }
    
    public string? MatchPath { get; set; }
    
    public string[] MatchHosts { get; set; }
    
    public string ClusterId { get; set; }
}