using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database.Entities.Configurations;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Database.Entities;

[EntityTypeConfiguration(typeof(LetsEncryptChallengeEntityConfiguration))]
public class LetsEncryptChallengeEntity : ITrackedEntity
{
    public Guid Id { get; set; }

    public string Domain { get; set; }

    public string Token { get; set; }

    public Guid LetsEncryptAccountId { get; set; }

    public LetsEncryptAccountEntity LetsEncryptAccount { get; set; }
    
    public DateTime CreatedDateUtc { get; set; }
    
    public DateTime UpdatedDateUtc { get; set; }
}