using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route("clusters")]
public class ClustersController : ControllerBase
{
    [HttpPost]
    public async Task<ApiResponse<Guid>> Create([FromBody] CreateClusterModel model, [FromServices] ClusterService clusterService)
    {
        var id = await clusterService.CreateAsync(model);
        return new ApiResponse<Guid>(id);
    }

    [HttpPut]
    public async Task Update([FromBody] UpdateClusterModel model, [FromServices] ClusterService clusterService)
    {
        await clusterService.UpdateAsync(model);
    }

    [HttpDelete("{clusterId:guid}")]
    public async Task Delete([FromRoute] Guid clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.DeleteAsync(clusterId);
    }

    [HttpGet("{clusterId:guid}")]
    public async Task<ClusterModel> Get([FromRoute] Guid clusterId, [FromServices] ClusterService clusterService)
    {
        var model = await clusterService.GetAsync(clusterId);
        return model;
    }

    [HttpGet]
    public async Task<ClusterModel[]> List([FromServices] ClusterService clusterService)
    {
        var models = await clusterService.ListAsync();
        return models;
    }

    [HttpPatch("{clusterId:guid}/enable")]
    public async Task Enable([FromRoute] Guid clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.SetEnabledAsync(clusterId, true);
    }
    
    [HttpPatch("{clusterId:guid}/disable")]
    public async Task Disable([FromRoute] Guid clusterId, [FromServices] ClusterService clusterService)
    {
        await clusterService.SetEnabledAsync(clusterId, false);
    }
}