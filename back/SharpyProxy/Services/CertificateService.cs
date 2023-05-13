using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Database.Entities;
using SharpyProxy.Extensions;
using SharpyProxy.Models.Certificate;

namespace SharpyProxy.Services;

public class CertificateService
{
    private readonly CertificateStore _certificateStore;

    private readonly AppDbContext _appDbContext;

    public CertificateService(CertificateStore certificateStore, AppDbContext appDbContext)
    {
        _certificateStore = certificateStore;
        _appDbContext = appDbContext;
    }

    public async Task<Guid> UploadAsync(UploadCertificateModel model)
    {
        var entity = new CertificateEntity
        {
            Name = model.Name,
            Pem = model.Pem,
            Key = model.Key
        };

        await _appDbContext.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        await _certificateStore.ReloadCertificatesAsync();

        return entity.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var deleted = await _appDbContext.Certificates
            .Where(certificate => certificate.Id == id)
            .ExecuteDeleteAsync();

        if (deleted > 0)
            await _certificateStore.ReloadCertificatesAsync();
    }

    public async Task<ListCertificateModel[]> ListAsync()
    {
        var entities = await _appDbContext.Certificates.ToArrayAsync();
        var models = entities.Select(entity =>
        {
            var certificate = X509Certificate2.CreateFromPem(entity.Pem, entity.Key);
            return new ListCertificateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Domain = certificate.GetDomain(),
                Expiration = certificate.GetExpiration()
            };
        }).ToArray();
        return models;
    }
}