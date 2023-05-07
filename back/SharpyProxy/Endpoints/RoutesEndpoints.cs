using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Route;
using SharpyProxy.Services;

namespace SharpyProxy.Endpoints;

public static class RoutesEndpoints
{
    public static IEndpointRouteBuilder MapRoutesEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .MapCreateRouteEndpoint()
            .MapDeleteRouteEndpoint()
            .MapGetRouteEndpoint();
    }
    
    private static IEndpointRouteBuilder MapCreateRouteEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/routes", async ([FromBody] CreateRouteModel model, [FromServices] RouteService routeService) =>
        {
            var id = await routeService.CreateAsync(model);
            return Results.Ok(id);
        });
        return builder;
    }
    
    private static IEndpointRouteBuilder MapDeleteRouteEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/routes/{routeId}", async ([FromRoute] string routeId, [FromServices] RouteService routeService) =>
        {
            await routeService.DeleteAsync(routeId);
            return Results.Ok();
        });
        return builder;
    }
    
    private static IEndpointRouteBuilder MapGetRouteEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/routes/{routeId}", async ([FromRoute] string routeId, [FromServices] RouteService routeService) =>
        {
            var model = await routeService.GetAsync(routeId);
            return Results.Ok(model);
        });
        return builder;
    }
}