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

    public async Task<string> CreateAsync(CreateClusterModel model)
    {
        var cluster = new ClusterEntity
        {
            Id = model.Id,
            Destinations = model.Destinations.Select(destination => new ClusterDestinationEntity
            {
                Id = destination.Id,
                Address = destination.Address
            }).ToList()
        };

        await _appDbContext.AddAsync(cluster);
        await _appDbContext.SaveChangesAsync();

        _proxyConfigProvider.Refresh();

        return cluster.Id;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _appDbContext.Clusters
            .Where(cluster => cluster.Id == id)
            .ExecuteDeleteAsync();

        if (deleted <= 0)
            return false;

        _proxyConfigProvider.Refresh();
        return true;
    }

    public async Task<ClusterModel> GetAsync(string id)
    {
        var cluster = await _appDbContext.Clusters
            .Include(cluster => cluster.Destinations)
            .FirstOrDefaultAsync(cluster => cluster.Id == id);

        if (cluster is null)
            throw new Exception();

        var model = new ClusterModel
        {
            Id = cluster.Id,
            Destinations = cluster.Destinations.Select(destination => new ClusterDestinationModel
            {
                Id = destination.Id,
                Address = destination.Address
            }).ToArray()
        };

        return model;
    }
}