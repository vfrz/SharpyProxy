using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Acme;
using SharpyProxy.Acme.Account;
using SharpyProxy.Acme.Challenge;
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

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CertificateService(CertificateStore certificateStore, AppDbContext appDbContext, IServiceScopeFactory serviceScopeFactory)
    {
        _certificateStore = certificateStore;
        _appDbContext = appDbContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task CreateManagedCertificateAsync(CreateManagedCertificateModel model)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var acmeClient = scope.ServiceProvider.GetRequiredService<AcmeClient>();

        await acmeClient.InitializeAsync();

        var letsEncryptAccountEntity = await _appDbContext.LetsEncryptAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Email == model.Email);

        AcmeAccount acmeAccount;
        if (letsEncryptAccountEntity is not null)
        {
            using var accountRsa = RSA.Create();
            accountRsa.ImportRSAPrivateKey(letsEncryptAccountEntity.RSABytes, out _);
            acmeAccount = await acmeClient.FindAccountByKeyAsync(accountRsa);
        }
        else
        {
            acmeAccount = await acmeClient.NewAccountAsync(model.Email);
            using var accountRsa = RSA.Create(acmeAccount.RSAParameters);
            letsEncryptAccountEntity = new LetsEncryptAccountEntity
            {
                Email = model.Email,
                RSABytes = accountRsa.ExportRSAPrivateKey()
            };
            await _appDbContext.AddAsync(letsEncryptAccountEntity);
            await _appDbContext.SaveChangesAsync();
        }

        var order = await acmeClient.NewOrderAsync(acmeAccount, model.Domain);
        var authorization = await acmeClient.FetchAuthorizationAsync(acmeAccount, order.AuthorizationUrls.Single());

        var challengeEntity = new LetsEncryptChallengeEntity
        {
            Domain = model.Domain,
            Token = authorization.HttpChallenge!.Token,
            LetsEncryptAccountId = letsEncryptAccountEntity.Id
        };
        await _appDbContext.AddAsync(challengeEntity);
        await _appDbContext.SaveChangesAsync();

        var challenge = await acmeClient.ChallengeReadyForValidationAsync(acmeAccount, authorization.HttpChallenge!.Url);

        while (challenge.Status is not AcmeChallengeStatus.Valid)
        {
            await Task.Delay(2000);
            challenge = await acmeClient.FetchChallengeAsync(acmeAccount, challenge.Url);
        }

        using var certificateKey = RSA.Create(2048);

        var finalizedOrder = await acmeClient.FinalizeOrderAsync(acmeAccount, order.FinalizeUrl, model.Domain, certificateKey);

        var certificateUrl = finalizedOrder.CertificateUrl;

        while (certificateUrl is null)
        {
            await Task.Delay(2000);
            var orderAfter = await acmeClient.FetchOrderAsync(acmeAccount, order.Url);
            certificateUrl = orderAfter.CertificateUrl;
        }

        var certificates = await acmeClient.DownloadCertificateAsync(acmeAccount, certificateUrl);

        var x509 = X509Certificate2.CreateFromPem(certificates.EndUserPemCertificate);

        var certificateEntity = new CertificateEntity
        {
            Name = model.Name,
            Type = CertificateType.Managed,
            Domain = x509.GetDomain(),
            ExpirationDateUtc = x509.GetExpiration().ToUniversalTime(),
            LetsEncryptAccountId = letsEncryptAccountEntity.Id,
            Key = certificateKey.ExportRSAPrivateKeyPem(),
            Pem = certificates.EndUserPemCertificate
        };

        await _appDbContext.AddAsync(certificateEntity);
        _appDbContext.Remove(challengeEntity);
        await _appDbContext.SaveChangesAsync();
        
        await _certificateStore.ReloadCertificatesAsync();
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