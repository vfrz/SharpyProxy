using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class ClusterEntityConfiguration : IEntityTypeConfiguration<ClusterEntity>
{
    public void Configure(EntityTypeBuilder<ClusterEntity> builder)
    {
        builder.HasKey(cluster => cluster.Id);

        builder.HasIndex(cluster => cluster.Name)
            .IsUnique();

        builder.Property(cluster => cluster.Name)
            .IsRequired();

        builder.HasMany(cluster => cluster.Routes)
            .WithOne(route => route.Cluster);

        builder.Property(cluster => cluster.Destinations)
            .HasColumnType("jsonb");
    }
}