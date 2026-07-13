using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Infrastructure.Configurations.WL
{
    public class WL_MasterEmailTemplateConfiguration : IEntityTypeConfiguration<WL_MasterEmailTemplate>
    {
        public void Configure(EntityTypeBuilder<WL_MasterEmailTemplate> builder)
        {
            builder.ToTable("WL_MasterEmailTemplate");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_WL_MasterEmailTemplate_UUID");

            builder.Property(x => x.EmailCredentialUUID)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.HasIndex(x => x.EmailCredentialUUID)
                .HasDatabaseName("IX_WL_MasterEmailTemplate_EmailCredentialUUID");

            builder.Property(x => x.EmailTemplateName)
                .HasMaxLength(200)
                .IsUnicode(true)
                .IsRequired();

            builder.HasIndex(x => x.EmailTemplateName)
                .HasDatabaseName("IX_WL_MasterEmailTemplate_EmailTemplateName");

            builder.Property(x => x.EmailSubject)
                .HasMaxLength(500)
                .IsUnicode(true)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(max)")
                .IsUnicode(true)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(x => x.TenantId)
                .HasColumnType("int")
                .IsRequired();

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_WL_MasterEmailTemplate_TenantId");

            // Composite index for tenant-specific queries
            builder.HasIndex(x => new { x.TenantId, x.EmailTemplateName })
                .IsUnique()
                .HasDatabaseName("IX_WL_MasterEmailTemplate_TenantId_EmailTemplateName");

            // Ignore NotMapped property
            builder.Ignore(x => x.TenantName);
        }
    }
}