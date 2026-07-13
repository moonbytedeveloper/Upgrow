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
    public class Pinned_ServicesConfiguration : IEntityTypeConfiguration<Pinned_Services>
    {
        public void Configure(EntityTypeBuilder<Pinned_Services> builder)
        {
            // Table name
            builder.ToTable("Pinned_Services");

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
                .IsRequired(true);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasDatabaseName("IX_Api_Category_UUID");

            // CategoryName
            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.ApiUUID)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.CustomerUUID)
                .HasDatabaseName("IX_Pinned_Services_CustomerUUID");

          
            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}