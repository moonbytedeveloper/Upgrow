using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations.AIX
{
    public class APIXVersionConfiguration : IEntityTypeConfiguration<ApiXVersion>
    {
        public void Configure(EntityTypeBuilder<ApiXVersion> builder)
        {
            // Table name
            builder.ToTable("ApiXVersion");

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
                .HasDatabaseName("IX_ApiXVersion_UUID");

            // CategoryName
            builder.Property(x => x.VersionNo)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasIndex(x => x.ApiCategoryUUID)
                .HasDatabaseName("IX_ApiXVersion_ApiCategoryUUID");


            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
    
