using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SharpyProxy.Database;
using Yarp.ReverseProxy.Configuration;

namespace SharpyProxy.Proxy;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private ILogger<CustomProxyConfigProvider> _logger;

    private CancellationTokenSource? _tokenSource;

    private readonly IServiceScopeFactory _scopeFactory;

    public CustomProxyConfigProvider(IServiceScopeFactory scopeFactory, ILogger<CustomProxyConfigProvider> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void Refresh()
    {
        _tokenSource?.Cancel();
    }

    public IProxyConfig GetConfig()
    {
        _logger.LogDebug("GetConfig is called");

        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var clusters = dbContext.Clusters
            .Include(cluster => cluster.Destinations)
            .ToList()
            .Select(cluster => new ClusterConfig
            {
                ClusterId = cluster.Id,
                Destinations = cluster.Destinations.ToDictionary(destination => destination.Id,
                    destination => new DestinationConfig
                    {
                        Address = destination.Address
                    })
            }).ToArray();

        var routes = dbContext.Routes
            .Select(route => new RouteConfig
            {
                RouteId = route.Id,
                Match = new RouteMatch
                {
                    Hosts = route.MatchHosts
                },
                ClusterId = route.ClusterId
            })
            .ToArray();

        _tokenSource = new CancellationTokenSource();

        return new CustomProxyConfig
        {
            Routes = routes,
            Clusters = clusters,
            ChangeToken = new CancellationChangeToken(_tokenSource.Token)
        };
    }
}