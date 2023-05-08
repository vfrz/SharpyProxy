using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Extensions;

namespace SharpyProxy.Services;

public class CertificateStore
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public IReadOnlyDictionary<string, X509Certificate2> LoadedCertificates => _loadedCertificates;

    private readonly ConcurrentDictionary<string, X509Certificate2> _loadedCertificates;

    public CertificateStore(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _loadedCertificates = new ConcurrentDictionary<string, X509Certificate2>();
    }

    public async Task ReloadCertificatesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var entities = await dbContext.Certificates
            .ToArrayAsync();
        var certificates = entities
            .Select(certificate => X509Certificate2.CreateFromPem(certificate.Pem, certificate.Key))
            .ToArray();

        foreach (var certificate in certificates)
        {
            _loadedCertificates.AddOrUpdate(certificate.GetDomain(), certificate, (_, _) => certificate);
        }

        var certificatesToRemove = _loadedCertificates.Keys.Except(certificates.Select(certificate => certificate.GetDomain()));
        foreach (var certificateToRemove in certificatesToRemove)
        {
            _loadedCertificates.Remove(certificateToRemove, out _);
        }
    }
}