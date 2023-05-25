using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Certificates;
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
        var certificate = X509Certificate2.CreateFromPem(model.Pem, model.Key);

        var entity = new CertificateEntity
        {
            Name = model.Name,
            Pem = model.Pem,
            Key = model.Key,
            Domain = certificate.GetDomain(),
            ExpirationDateUtc = certificate.GetExpiration().ToUniversalTime(),
            Type = CertificateType.Unmanaged
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
        var models = await _appDbContext.Certificates
            .AsNoTracking()
            .Select(entity => new ListCertificateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Domain = entity.Domain,
                Expiration = entity.ExpirationDateUtc,
                Type = entity.Type
            }).ToArrayAsync();
        return models;
    }
}