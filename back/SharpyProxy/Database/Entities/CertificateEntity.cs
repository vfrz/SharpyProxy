using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(CertificateEntityConfiguration))]
public class CertificateEntity : ITrackedEntity
{
    public string Id { get; set; }

    public string Pem { get; set; }

    public string Key { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
}