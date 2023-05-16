using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SharpyProxy.Models;
using SharpyProxy.Services;
using SharpyProxy.Setup;

IServiceProvider? serviceProvider = null;

var builder = WebApplication.CreateBuilder(args)
    .SetupDatabase()
    .SetupProxyServices();

builder.WebHost.ConfigureKestrel(kestrelOptions =>
{
    kestrelOptions.ListenAnyIP(4343, options =>
    {
        options.UseHttps(httpsOptions =>
        {
            httpsOptions.ServerCertificateSelector = (context, name) =>
            {
                if (serviceProvider is null)
                    throw new Exception("SharpyProxy is not ready to take requests");

                if (string.IsNullOrEmpty(name))
                {
                    //TODO return default certificate
                    return null;
                }
                    
                var certificateStore = serviceProvider.GetRequiredService<CertificateStore>();
                if (certificateStore.LoadedCertificates.TryGetValue(name, out var certificate))
                    return certificate;

                return null;
            };
        });
    });
    kestrelOptions.ListenAnyIP(8080);
});

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
app.MapControllers();
app.MapReverseProxy(pipeline =>
{
    pipeline.Use((context, next) =>
    {
        //TODO Implement ACME HTTP verification here
        return next();
    });
});

serviceProvider = app.Services;
var certificateStore = app.Services.GetRequiredService<CertificateStore>();
await certificateStore.ReloadCertificatesAsync();

app.Run();