namespace SharpyProxy.Models.Route;

public class UpdateRouteModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Enabled { get; set; }
    
    public string? MatchPath { get; set; }
    
    public string[]? MatchHosts { get; set; }
    
    public Guid ClusterId { get; set; }
}