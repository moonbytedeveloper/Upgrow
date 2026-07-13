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
    public class ApiConfiguration : IEntityTypeConfiguration<Master_Api>
    {
        public void Configure(EntityTypeBuilder<Master_Api> builder)
        {
            // Table name
            builder.ToTable("Master_Api");

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
                .HasDatabaseName("IX_Api_UUID");

            // ApiName
            builder.Property(x => x.ApiName)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(x => x.ApiName)
                .HasDatabaseName("IX_Api_ApiName");

            // Code
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Api_Code");

            // ApiCategoryUUID
            builder.Property(x => x.ApiCategoryUUID)
                .HasMaxLength(150)
                .IsUnicode(false);

            // ShortDescription
            builder.Property(x => x.ShortDescription)
                .HasMaxLength(350)
                .IsRequired(false);

            // DisplayOrder
            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(x => x.IsMultipleEndPoint)
                .IsRequired()
                .HasDefaultValue(false)
                .ValueGeneratedNever();
        }
    }
}