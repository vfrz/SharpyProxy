using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/clusters")]
public class ClustersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateClusterModel model, [FromServices] ClusterService clusterService)
    {
        await clusterService.CreateAsync(model);
        return Ok();
    }

    [HttpDelete("{clusterId}")]
    public async Task<ActionResult> Delete([FromRoute] string clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.DeleteAsync(clusterId);
        return Ok();
    }

    [HttpGet("{clusterId}")]
    public async Task<ActionResult<ClusterModel>> Get([FromRoute] string clusterId, [FromServices] ClusterService clusterService)
    {
        var model = await clusterService.GetAsync(clusterId);
        return Ok(model);
    }

    [HttpGet]
    public async Task<ActionResult<ClusterModel[]>> List([FromServices] ClusterService clusterService)
    {
        var models = await clusterService.ListAsync();
        return Ok(models);
    }

    [HttpPatch("{clusterId}/enable")]
    public async Task<ActionResult> Enable([FromRoute] string clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.SetEnabledAsync(clusterId, true);
        return Ok();
    }
    
    [HttpPatch("{clusterId}/disable")]
    public async Task<ActionResult> Disable([FromRoute] string clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.SetEnabledAsync(clusterId, false);
        return Ok();
    }
}