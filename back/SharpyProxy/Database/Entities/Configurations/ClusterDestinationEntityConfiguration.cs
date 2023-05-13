using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class ClusterDestinationEntityConfiguration : IEntityTypeConfiguration<ClusterDestinationEntity>
{
    public void Configure(EntityTypeBuilder<ClusterDestinationEntity> builder)
    {
        builder.HasKey(destination => destination.Id);

        builder.HasIndex(destination => destination.Name)
            .IsUnique();
        
        builder.Property(destination => destination.Name)
            .IsRequired();

        builder.HasOne(destination => destination.Cluster)
            .WithMany(cluster => cluster.Destinations)
            .HasForeignKey(destination => destination.ClusterId)
            .IsRequired();
    }
}