using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities;

namespace SharpyProxy.Database;

public class AppDbContext : DbContext
{
    public DbSet<RouteEntity> Routes { get; set; }
    
    public DbSet<ClusterEntity> Clusters { get; set; }
    
    public DbSet<ClusterDestinationEntity> ClusterDestinations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}