using SharpyProxy.Endpoints;

namespace SharpyProxy.Setup;

public static class SetupEndpointsExtensions
{
    public static WebApplication SetupEndpoints(this WebApplication application)
    {
        application.MapGroup("/.proxy-api")
            .MapRoutesEndpoints()
            .MapClustersEndpoints();
        return application;
    }
}