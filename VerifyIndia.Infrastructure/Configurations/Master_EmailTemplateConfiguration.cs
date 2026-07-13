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
    public class Master_EmailTemplateConfiguration : IEntityTypeConfiguration<Master_EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<Master_EmailTemplate> builder)
        {
            // Table name
            builder.ToTable("Master_EmailTemplate");

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
                .HasDatabaseName("IX_Master_EmailTemplate_UUID");

            // Title
            builder.Property(x => x.EmailCredentialUUID)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.EmailTemplateName)
                .HasDatabaseName("IX_Master_EmailTemplate_EmailTemplate_Name");

            // ShortTitle
            builder.Property(x => x.EmailSubject)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
              .HasMaxLength(50)
              .IsRequired();



            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}
    
