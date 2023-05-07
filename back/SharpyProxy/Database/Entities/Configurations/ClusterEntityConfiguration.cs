using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class ClusterEntityConfiguration : IEntityTypeConfiguration<ClusterEntity>
{
    public void Configure(EntityTypeBuilder<ClusterEntity> builder)
    {
        builder.HasKey(cluster => cluster.Id);
        
        builder.HasMany(cluster => cluster.Routes)
            .WithOne(route => route.Cluster);

        builder.HasMany(cluster => cluster.Destinations)
            .WithOne(destination => destination.Cluster);
    }
}