using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace SharpyProxy.Proxy;

public class CustomProxyConfig : IProxyConfig
{
    public IReadOnlyList<RouteConfig> Routes { get; init; }

    public IReadOnlyList<ClusterConfig> Clusters { get; init; }

    public IChangeToken ChangeToken { get; init; }
}