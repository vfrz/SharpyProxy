using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class LetsEncryptAccountEntityConfiguration : IEntityTypeConfiguration<LetsEncryptAccountEntity>
{
    public void Configure(EntityTypeBuilder<LetsEncryptAccountEntity> builder)
    {
        builder.HasKey(account => account.Id);

        builder.HasIndex(account => account.Email)
            .IsUnique();
        
        builder.Property(account => account.Email)
            .IsRequired();
    }
}