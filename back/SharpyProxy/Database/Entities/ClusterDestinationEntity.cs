using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(ClusterDestinationEntityConfiguration))]
public class ClusterDestinationEntity
{
    public string Id { get; set; }

    public string Address { get; set; }

    public string ClusterId { get; set; }

    public ClusterEntity Cluster { get; set; }
}