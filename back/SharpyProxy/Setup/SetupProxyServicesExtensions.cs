using FluentValidation;
using SharpyProxy.Acme;
using SharpyProxy.Certificates;
using SharpyProxy.Proxy;
using SharpyProxy.Services;
using Yarp.ReverseProxy.Configuration;

namespace SharpyProxy.Setup;

public static class SetupProxyServicesExtensions
{
    public static WebApplicationBuilder SetupProxyServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton<CustomProxyConfigProvider>()
            .AddSingleton<IProxyConfigProvider>(provider => provider.GetRequiredService<CustomProxyConfigProvider>())
            .AddReverseProxy();

        services.AddScoped<RouteService>()
            .AddScoped<ClusterService>()
            .AddScoped<CertificateService>()
            .AddScoped<CoreService>()
            .AddSingleton<CertificateStore>()
            .AddSingleton(new AcmeSettings
            {
                ServerUrl = AcmeKnownServers.LetsEncryptV2Url
            })
            .AddScoped<AcmeClient>();

        services.AddValidatorsFromAssembly(typeof(SetupProxyServicesExtensions).Assembly);
        
        return builder;
    }
}