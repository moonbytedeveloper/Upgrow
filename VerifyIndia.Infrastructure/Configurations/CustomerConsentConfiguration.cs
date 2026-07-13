using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class CustomerConsentConfiguration : IEntityTypeConfiguration<CustomerConsent>
    {
        public void Configure(EntityTypeBuilder<CustomerConsent> builder)
        {
            builder.ToTable("CustomerConsent");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();          

            builder.Property(x => x.PolicyVersion)
                .HasMaxLength(10)
                .IsRequired(false);

           

            builder.HasIndex(x => x.CustomerUUID)
                .HasDatabaseName("IX_CustomerConsent_CustomerUUID");


        }
    }
}
