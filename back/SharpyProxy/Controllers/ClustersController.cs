using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/clusters")]
public class ClustersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateClusterModel model, [FromServices] ClusterService clusterService)
    {
        var id = await clusterService.CreateAsync(model);
        return Ok(id);
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
}