using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Proxy;
using SharpyProxy.Setup;

var builder = WebApplication.CreateBuilder(args)
    .SetupDatabase()
    .SetupProxyServices();

var app = builder.Build()
    .SetupEndpoints();
app.MapGet("/.proxy-api/refresh", ([FromServices] CustomProxyConfigProvider proxyConfigProvider) =>
{
    proxyConfigProvider.Refresh();
    return Results.Text("Config refreshed!");
});
app.MapReverseProxy();
app.Run();