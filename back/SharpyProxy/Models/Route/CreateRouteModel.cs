namespace SharpyProxy.Models.Route;

public class CreateRouteModel
{
    public string Name { get; set; }
    
    public string? MatchPath { get; set; }
    
    public string[] MatchHosts { get; set; }
    
    public Guid ClusterId { get; set; }
}