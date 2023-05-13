using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(ClusterDestinationEntityConfiguration))]
public class ClusterDestinationEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string Address { get; set; }

    public Guid ClusterId { get; set; }

    public ClusterEntity Cluster { get; set; }
}