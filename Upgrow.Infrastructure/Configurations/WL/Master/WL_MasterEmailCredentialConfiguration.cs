using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class WL_MasterEmailCredentialConfiguration : IEntityTypeConfiguration<WL_MasterEmailCredential>
    {
        public void Configure(EntityTypeBuilder<WL_MasterEmailCredential> builder)
        {
            // Table name
            builder.ToTable("WL_MasterEmailCredential");

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
                .HasDatabaseName("IX_Master_EmailCredential_UUID");

            // Title
            builder.Property(x => x.EmailAddress)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => x.HostServiceProvider)
                .HasDatabaseName("IX_Master_EmailCredential_Host_ServiceProvider");

            // ShortTitle
            builder.Property(x => x.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.SMTP)
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.Port)
            .HasMaxLength(50)
            .IsRequired();

            builder.Property(x => x.TenantId)
               .HasColumnType("int")
               .IsRequired();

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}
