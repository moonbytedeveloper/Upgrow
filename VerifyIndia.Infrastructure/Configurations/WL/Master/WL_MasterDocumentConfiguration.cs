using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Infrastructure.Configurations.WL.Master
{
    public class WL_MasterDocumentConfiguration : IEntityTypeConfiguration<WL_MasterDocument>
    {
        public void Configure(EntityTypeBuilder<WL_MasterDocument> builder)
        {
            // Table name
            builder.ToTable("WL_MasterDocument");

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
                .HasDatabaseName("IX_WL_MasterDocument_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_WL_MasterDocument_Title");

            // FileType
            builder.Property(x => x.FileType)
                .HasMaxLength(50)
                .IsRequired(false);

            // Path
            builder.Property(x => x.Path)
                .HasMaxLength(400)
                .IsRequired(false);

            // IsActive - Soft delete flag (if present in BaseEntity)
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
   

