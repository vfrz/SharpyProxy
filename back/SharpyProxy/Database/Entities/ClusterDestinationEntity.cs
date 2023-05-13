using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(ClusterDestinationEntityConfiguration))]
public class ClusterDestinationEntity : ITrackedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public Guid ClusterId { get; set; }

    public ClusterEntity Cluster { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
}