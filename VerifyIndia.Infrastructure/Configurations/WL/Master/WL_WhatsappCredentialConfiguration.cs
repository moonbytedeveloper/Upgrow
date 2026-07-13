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
    public class WL_WhatsappCredentialConfiguration : IEntityTypeConfiguration<WL_WhatsappCredential>
    {
        public void Configure(EntityTypeBuilder<WL_WhatsappCredential> builder)
        {
            // Table name
            builder.ToTable("WL_WhatsappCredential");

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
                .HasDatabaseName("IX_WL_WhatsappCredential_UUID");

            builder.Property(x => x.ApiKey)
                .HasMaxLength(200)
                .IsRequired(false)
                .IsUnicode(true)
                .HasColumnType("nvarchar(200)");

            builder.Property(x => x.SecretKey)
                .HasMaxLength(200)
                .IsRequired(false)
                .IsUnicode(true)
                .HasColumnType("nvarchar(200)");

            builder.Property(x => x.MobileNumber)
                .HasMaxLength(50)
                .IsRequired(false)
                .IsUnicode(true)
                .HasColumnType("nvarchar(50)");

            builder.Property(x => x.SenderName)
                .HasMaxLength(100)
                .IsRequired(false)
                .IsUnicode(true)
                .HasColumnType("nvarchar(100)");


            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT

        }
    }
}
    
   