using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(ClusterEntityConfiguration))]
public class ClusterEntity
{
    public string Id { get; set; }

    public List<RouteEntity> Routes { get; set; }

    public List<ClusterDestinationEntity> Destinations { get; set; }
}