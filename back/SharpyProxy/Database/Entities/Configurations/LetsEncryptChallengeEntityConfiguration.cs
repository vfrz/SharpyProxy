using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class LetsEncryptChallengeEntityConfiguration : IEntityTypeConfiguration<LetsEncryptChallengeEntity>
{
    public void Configure(EntityTypeBuilder<LetsEncryptChallengeEntity> builder)
    {
        builder.HasKey(challenge => challenge.Id);

        builder.Property(challenge => challenge.Domain)
            .IsRequired();
        
        builder.Property(challenge => challenge.Token)
            .IsRequired();
        
        builder.HasOne(challenge => challenge.LetsEncryptAccount)
            .WithMany(account => account.Challenges)
            .HasForeignKey(challenge => challenge.LetsEncryptAccountId)
            .IsRequired();
    }
}