using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.WL.Master;

namespace Upgrow.Infrastructure.Configurations.WL.Master
{
    public class WL_SMSCredentialConfiguration : IEntityTypeConfiguration<WL_SMSCredential>
    {
        public void Configure(EntityTypeBuilder<WL_SMSCredential> builder)
        {
            // Table name
            builder.ToTable("WL_SMSCredential");

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
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_WL_SMSCredential_UUID");

            // Title
            builder.Property(x => x.ApiKey)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => x.ApiKey)
                .HasDatabaseName("IX_WL_SMSCredential_ApiKey");

            // ShortTitle
            builder.Property(x => x.SenderId)
                .HasMaxLength(20)
                .IsRequired(false);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT


            // Query filter for soft delete (optional - apply if you want automatic filtering)
            // builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}
 