using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(RouteEntityConfiguration))]
public class RouteEntity : ITrackedEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string? MatchPath { get; set; }

    public List<string>? MatchHosts { get; set; }

    public Guid ClusterId { get; set; }

    public ClusterEntity Cluster { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
}