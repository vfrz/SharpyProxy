using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SharpyProxy.Certificates;
using SharpyProxy.Middlewares;
using SharpyProxy.Models;
using SharpyProxy.Settings;
using SharpyProxy.Setup;

var builder = WebApplication.CreateBuilder(args)
    .SetupDatabase()
    .SetupServices();

var networkSettings = builder.Configuration.GetSection(NetworkSettings.Section).Get<NetworkSettings>() ?? new NetworkSettings();

builder.WebHost.ConfigureKestrel(kestrelOptions =>
{
    kestrelOptions.ListenAnyIP(networkSettings.ProxyHttpsPort, options =>
    {
        options.UseHttps(httpsOptions =>
        {
            httpsOptions.ServerCertificateSelector = (_, name) =>
            {
                var certificateStore = options.ApplicationServices.GetRequiredService<CertificateStore>();

                if (name is not null && certificateStore.LoadedCertificates.TryGetValue(name, out var certificate))
                    return certificate;

                return null;
            };
        });
    });
    kestrelOptions.ListenAnyIP(networkSettings.ProxyHttpPort);
    kestrelOptions.ListenAnyIP(networkSettings.ApiPort);
});

builder.Services.AddScoped<AcmeChallengeMiddleware>();

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("Default", policyBuilder =>
{
    policyBuilder
        .AllowCredentials()
        .WithOrigins(networkSettings.DashboardUrl)
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>()!;
        var exception = exceptionHandler.Error;

        context.Response.StatusCode = (int) HttpStatusCode.OK;
        await context.Response.WriteAsJsonAsync(new ApiResponse
        {
            Success = false,
            Message = exception.Message
        });
    });
});

app.UseCors("Default");
app.MapControllers().RequireHost($"*:{networkSettings.ApiPort}");
app.MapReverseProxy(pipeline =>
{
    pipeline.UseMiddleware<AcmeChallengeMiddleware>();
});

var certificateStore = app.Services.GetRequiredService<CertificateStore>();
await certificateStore.ReloadCertificatesAsync();

app.Run();