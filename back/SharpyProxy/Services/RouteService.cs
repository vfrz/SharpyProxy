using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Database.Entities;
using SharpyProxy.Models.Route;
using SharpyProxy.Proxy;

namespace SharpyProxy.Services;

public class RouteService
{
    private readonly AppDbContext _appDbContext;
    private readonly CustomProxyConfigProvider _proxyConfigProvider;

    public RouteService(AppDbContext appDbContext, CustomProxyConfigProvider proxyConfigProvider)
    {
        _appDbContext = appDbContext;
        _proxyConfigProvider = proxyConfigProvider;
    }

    public async Task<Guid> CreateAsync(CreateRouteModel model)
    {
        var route = new RouteEntity
        {
            Name = model.Name,
            MatchPath = model.MatchPath,
            MatchHosts = model.MatchHosts.ToList(),
            ClusterId = model.ClusterId,
            Enabled = true
        };

        await _appDbContext.AddAsync(route);
        await _appDbContext.SaveChangesAsync();

        _proxyConfigProvider.Refresh();

        return route.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _appDbContext.Routes
            .Where(route => route.Id == id)
            .ExecuteDeleteAsync();

        if (deleted <= 0)
            return false;

        _proxyConfigProvider.Refresh();
        return true;
    }

    public async Task<RouteModel> GetAsync(Guid id)
    {
        var route = await _appDbContext.Routes
            .FirstOrDefaultAsync(route => route.Id == id);

        if (route is null)
            throw new Exception($"No route found with id: {id}");

        var model = new RouteModel
        {
            Id = route.Id,
            Name = route.Name,
            ClusterId = route.ClusterId,
            MatchHosts = route.MatchHosts.ToArray(),
            MatchPath = route.MatchPath,
            Enabled = route.Enabled
        };

        return model;
    }

    public async Task<ListRouteModel[]> ListAsync()
    {
        var routes = await _appDbContext.Routes
            .Select(route => new ListRouteModel
            {
                Id = route.Id,
                Name = route.Name,
                ClusterId = route.ClusterId,
                ClusterName = route.Cluster.Name,
                MatchHosts = route.MatchHosts.ToArray(),
                MatchPath = route.MatchPath,
                Enabled = route.Enabled
            }).ToArrayAsync();

        return routes;
    }
}