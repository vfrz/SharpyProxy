using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Database.Entities;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Models.Cluster.Destination;
using SharpyProxy.Proxy;

namespace SharpyProxy.Services;

public class ClusterService
{
    private readonly AppDbContext _appDbContext;
    private readonly CustomProxyConfigProvider _proxyConfigProvider;

    public ClusterService(AppDbContext appDbContext, CustomProxyConfigProvider proxyConfigProvider)
    {
        _appDbContext = appDbContext;
        _proxyConfigProvider = proxyConfigProvider;
    }

    public async Task<Guid> CreateAsync(CreateClusterModel model)
    {
        var cluster = new ClusterEntity
        {
            Name = model.Name,
            Enabled = model.Enabled,
            Destinations = model.Destinations.Select(destination => new ClusterDestinationEntity
            {
                Name = destination.Name,
                Address = destination.Address
            }).ToList()
        };

        await _appDbContext.AddAsync(cluster);
        await _appDbContext.SaveChangesAsync();

        _proxyConfigProvider.Refresh();

        return cluster.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _appDbContext.Clusters
            .Where(cluster => cluster.Id == id)
            .ExecuteDeleteAsync();

        if (deleted <= 0)
            return false;

        _proxyConfigProvider.Refresh();
        return true;
    }

    public async Task<ClusterModel> GetAsync(Guid id)
    {
        var cluster = await _appDbContext.Clusters
            .Include(cluster => cluster.Destinations)
            .FirstOrDefaultAsync(cluster => cluster.Id == id);

        if (cluster is null)
            throw new Exception($"No cluster found with id: {id}");

        var model = new ClusterModel
        {
            Id = cluster.Id,
            Name = cluster.Name,
            Enabled = cluster.Enabled,
            Destinations = cluster.Destinations.Select(destination => new ClusterDestinationModel
            {
                Id = destination.Id,
                Address = destination.Address
            }).ToArray()
        };

        return model;
    }

    public async Task<ClusterModel[]> ListAsync()
    {
        var clusters = await _appDbContext.Clusters
            .Include(cluster => cluster.Destinations)
            .ToListAsync();

        var models = clusters.Select(cluster => new ClusterModel
        {
            Id = cluster.Id,
            Name = cluster.Name,
            Enabled = cluster.Enabled,
            Destinations = cluster.Destinations.Select(destination => new ClusterDestinationModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Address = destination.Address
            }).ToArray()
        }).ToArray();

        return models;
    }

    public async Task SetEnabledAsync(Guid id, bool enabled)
    {
        var cluster = await _appDbContext.Clusters
            .FirstOrDefaultAsync(cluster => cluster.Id == id);
        
        if (cluster is null)
            throw new Exception($"No cluster found with id: {id}");
        
        cluster.Enabled = enabled;
        await _appDbContext.SaveChangesAsync();
    }
}