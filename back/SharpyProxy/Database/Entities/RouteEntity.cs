using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(RouteEntityConfiguration))]
public class RouteEntity
{
    public string Id { get; set; }
    
    public string? MatchPath { get; set; }
    
    public List<string> MatchHosts { get; set; }
    
    public string ClusterId { get; set; }
    
    public ClusterEntity Cluster { get; set; }
}