using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class RouteEntityConfiguration : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.HasKey(route => route.Id);

        builder.HasIndex(route => route.Name)
            .IsUnique();
        
        builder.Property(route => route.Name)
            .IsRequired();
        
        builder.HasOne(route => route.Cluster)
            .WithMany(cluster => cluster.Routes)
            .HasForeignKey(route => route.ClusterId)
            .IsRequired();
    }
}