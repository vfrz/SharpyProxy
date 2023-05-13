using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Route;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/routes")]
public class RoutesController : ControllerBase
{
    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateRouteModel model, [FromServices] RouteService routeService)
    {
        var id = await routeService.CreateAsync(model);
        return id;
    }

    [HttpDelete("{routeId:guid}")]
    public async Task Delete([FromRoute] Guid routeId, [FromServices] RouteService routeService)
    {
        await routeService.DeleteAsync(routeId);
    }

    [HttpGet("{routeId:guid}")]
    public async Task<RouteModel> Get([FromRoute] Guid routeId, [FromServices] RouteService routeService)
    {
        var model = await routeService.GetAsync(routeId);
        return model;
    }
    
    [HttpGet]
    public async Task<ListRouteModel[]> List([FromServices] RouteService routeService)
    {
        var models = await routeService.ListAsync();
        return models;
    }
}