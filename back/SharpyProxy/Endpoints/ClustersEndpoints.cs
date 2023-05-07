using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Services;

namespace SharpyProxy.Endpoints;

public static class ClustersEndpoints
{
    public static IEndpointRouteBuilder MapClustersEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .MapCreateClusterEndpoint()
            .MapDeleteClusterEndpoint()
            .MapGetClusterEndpoint();
    }
    
    private static IEndpointRouteBuilder MapCreateClusterEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/clusters", async ([FromBody] CreateClusterModel model, [FromServices] ClusterService clusterService) =>
        {
            var id = await clusterService.CreateAsync(model);
            return Results.Ok(id);
        });
        return builder;
    }
    
    private static IEndpointRouteBuilder MapDeleteClusterEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/clusters/{clusterId}", async ([FromRoute] string clusterId, [FromServices] ClusterService clusterService) =>
        {
            await clusterService.DeleteAsync(clusterId);
            return Results.Ok();
        });
        return builder;
    }
    
    private static IEndpointRouteBuilder MapGetClusterEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/clusters/{clusterId}", async ([FromRoute] string clusterId, [FromServices] ClusterService clusterService) =>
        {
            var model = await clusterService.GetAsync(clusterId);
            return Results.Ok(model);
        });
        return builder;
    }
}