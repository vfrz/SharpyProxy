using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class ClusterDestinationEntityConfiguration : IEntityTypeConfiguration<ClusterDestinationEntity>
{
    public void Configure(EntityTypeBuilder<ClusterDestinationEntity> builder)
    {
        builder.HasKey(destination => new
        {
            destination.Id,
            destination.ClusterId
        });

        builder.HasOne(destination => destination.Cluster)
            .WithMany(cluster => cluster.Destinations)
            .HasForeignKey(destination => destination.ClusterId)
            .IsRequired();
    }
}