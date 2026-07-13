using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class ProviderApisConfiguration : IEntityTypeConfiguration<Provider_Apis>
    {
        public void Configure(EntityTypeBuilder<Provider_Apis> builder)
        {
            // Table name
            builder.ToTable("Provider_Apis");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.UUID)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasDatabaseName("IX_ProviderApis_UUID");

            // ApiUUID - Reference to API
            builder.Property(x => x.ApiUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.ApiUUID)
                .HasDatabaseName("IX_ProviderApis_ApiUUID");

            // ProviderUUID - Reference to Provider
            builder.Property(x => x.ProviderUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.ProviderUUID)
                .HasDatabaseName("IX_ProviderApis_ProviderUUID");

            // StepCode - Process step identifier
            

            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}