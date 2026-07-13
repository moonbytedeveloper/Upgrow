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
    public class WL_MasterPermissionGroupConfiguration : IEntityTypeConfiguration<WL_MasterPermissionGroup>
    {
        public void Configure(EntityTypeBuilder<WL_MasterPermissionGroup> builder)
        {
            // Table name
            builder.ToTable("WL_MasterPermissionGroup");

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
                .HasDatabaseName("WL_MasterPermissionGroup_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("WL_MasterPermissionGroup_Title");

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}

