using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(LetsEncryptAccountEntityConfiguration))]
public class LetsEncryptAccountEntity : ITrackedEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public byte[] RSABytes { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime UpdatedDateUtc { get; set; }
    
    public List<CertificateEntity> Certificates { get; set; }
    
    public List<LetsEncryptChallengeEntity> Challenges { get; set; }
}