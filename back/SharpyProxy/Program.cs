using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SharpyProxy.Certificates;
using SharpyProxy.Middlewares;
using SharpyProxy.Models;
using SharpyProxy.Settings;
using SharpyProxy.Setup;

var builder = WebApplication.CreateBuilder(args)
    .SetupDatabase()
    .SetupProxyServices();

var portsSettings = builder.Configuration.GetSection(PortsSettings.Section).Get<PortsSettings>() ?? new PortsSettings();

builder.WebHost.ConfigureKestrel(kestrelOptions =>
{
    kestrelOptions.ListenAnyIP(portsSettings.Https, options =>
    {
        options.UseHttps(httpsOptions =>
        {
            httpsOptions.ServerCertificateSelector = (context, name) =>
            {
                var certificateStore = options.ApplicationServices.GetRequiredService<CertificateStore>();

                if (string.IsNullOrEmpty(name))
                {
                    //TODO return default certificate
                    return null;
                }
                    
                if (certificateStore.LoadedCertificates.TryGetValue(name, out var certificate))
                    return certificate;

                return null;
            };
        });
    });
    kestrelOptions.ListenAnyIP(portsSettings.Http);
    kestrelOptions.ListenAnyIP(portsSettings.Api);
});

builder.Services.AddScoped<AcmeChallengeMiddleware>();

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("Default", policyBuilder =>
{
    policyBuilder
        .AllowCredentials()
        .WithOrigins("http://localhost:3000")
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
app.MapControllers().RequireHost($"*:{portsSettings.Api}");
app.MapReverseProxy(pipeline =>
{
    pipeline.UseMiddleware<AcmeChallengeMiddleware>();
});

var certificateStore = app.Services.GetRequiredService<CertificateStore>();
await certificateStore.ReloadCertificatesAsync();

app.Run();