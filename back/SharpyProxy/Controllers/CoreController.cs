using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Core;
using SharpyProxy.Proxy;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/core")]
public class CoreController : ControllerBase
{
    [HttpGet("refresh")]
    public ActionResult Refresh([FromServices] CustomProxyConfigProvider proxyConfigProvider)
    {
        proxyConfigProvider.Refresh();
        return Ok();
    }

    [HttpGet("stats")]
    public async Task<ActionResult<StatsModel>> GetStats([FromServices] CoreService coreService)
    {
        var stats = await coreService.GetStatsAsync();
        return Ok(stats);
    }
}