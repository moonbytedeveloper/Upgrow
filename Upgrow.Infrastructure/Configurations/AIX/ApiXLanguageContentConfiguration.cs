using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations.AIX
{
    public class ApiXLanguageContentConfiguration : IEntityTypeConfiguration<ApiXLanguageContent>
    {
        public void Configure(EntityTypeBuilder<ApiXLanguageContent> builder)
        {
            // Table name
            builder.ToTable("ApiXLanguageContent");

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
                .HasDatabaseName("IX_ApiXLanguageContent_UUID");

            // CategoryName
            builder.Property(x => x.ApiXVersionUUID)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasIndex(x => x.LanguageContent)
                .HasDatabaseName("IX_ApiXLanguageContent_LanguageContent");


            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
    
