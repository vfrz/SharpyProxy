using Microsoft.EntityFrameworkCore;
using SharpyProxy.Certificates;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(CertificateEntityConfiguration))]
public class CertificateEntity : ITrackedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Pem { get; set; }

    public string Key { get; set; }

    public CertificateType Type { get; set; }

    public string Domain { get; set; }

    public DateTime ExpirationDateUtc { get; set; }

    public Guid? LetsEncryptAccountId { get; set; }

    public LetsEncryptAccountEntity? LetsEncryptAccount { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
}