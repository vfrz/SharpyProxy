using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Proxy;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/core")]
public class CoreController : ControllerBase
{
    [HttpGet]
    public ActionResult Refresh([FromServices] CustomProxyConfigProvider proxyConfigProvider)
    {
        proxyConfigProvider.Refresh();
        return Ok();
    }
}