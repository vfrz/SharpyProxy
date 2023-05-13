using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(ClusterEntityConfiguration))]
public class ClusterEntity : ITrackedEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public List<RouteEntity> Routes { get; set; }

    public List<ClusterDestinationEntity> Destinations { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
}