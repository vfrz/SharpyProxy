using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharpyProxy.Database.Entities.Configurations;

public class CertificateEntityConfiguration : IEntityTypeConfiguration<CertificateEntity>
{
    public void Configure(EntityTypeBuilder<CertificateEntity> builder)
    {
        builder.HasKey(certificate => certificate.Id);

        builder.HasIndex(certificate => certificate.Name)
            .IsUnique();

        builder.Property(certificate => certificate.Name)
            .IsRequired();

        builder.Property(certificate => certificate.Pem)
            .IsRequired();

        builder.Property(certificate => certificate.Key)
            .IsRequired();
    }
}