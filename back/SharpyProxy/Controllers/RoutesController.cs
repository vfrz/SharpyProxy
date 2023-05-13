using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Route;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/routes")]
public class RoutesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateRouteModel model, [FromServices] RouteService routeService)
    {
        var id = await routeService.CreateAsync(model);
        return Ok(id);
    }

    [HttpDelete("{routeId:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid routeId, [FromServices] RouteService routeService)
    {
        await routeService.DeleteAsync(routeId);
        return Ok();
    }

    [HttpGet("{routeId:guid}")]
    public async Task<ActionResult<RouteModel>> Get([FromRoute] Guid routeId, [FromServices] RouteService routeService)
    {
        var model = await routeService.GetAsync(routeId);
        return Ok(model);
    }
    
    [HttpGet]
    public async Task<ActionResult<RouteModel[]>> List([FromServices] RouteService routeService)
    {
        var models = await routeService.ListAsync();
        return Ok(models);
    }
}