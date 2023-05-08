using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SharpyProxy.Database;
using Yarp.ReverseProxy.Configuration;

namespace SharpyProxy.Proxy;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private ILogger<CustomProxyConfigProvider> _logger;

    private CancellationTokenSource? _tokenSource;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CustomProxyConfigProvider(IServiceScopeFactory serviceScopeFactory, ILogger<CustomProxyConfigProvider> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public void Refresh()
    {
        _tokenSource?.Cancel();
    }

    public IProxyConfig GetConfig()
    {
        _logger.LogDebug("GetConfig is called");

        using var scope = _serviceScopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var clusters = dbContext.Clusters
            .Include(cluster => cluster.Destinations)
            .Where(cluster => cluster.Enabled)
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
            .Where(route => route.Enabled)
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