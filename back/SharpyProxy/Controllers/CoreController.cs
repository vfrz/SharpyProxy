using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Core;
using SharpyProxy.Proxy;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route("core")]
public class CoreController : ControllerBase
{
    [HttpGet("refresh")]
    public void Refresh([FromServices] CustomProxyConfigProvider proxyConfigProvider)
    {
        proxyConfigProvider.Refresh();
    }

    [HttpGet("stats")]
    public async Task<StatsModel> GetStats([FromServices] CoreService coreService)
    {
        var stats = await coreService.GetStatsAsync();
        return stats;
    }
}