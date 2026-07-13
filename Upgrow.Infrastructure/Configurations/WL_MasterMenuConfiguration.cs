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
    public class WL_MasterMenuConfiguration : IEntityTypeConfiguration<WL_MasterMenu>
    {
        public void Configure(EntityTypeBuilder<WL_MasterMenu> builder)
        {
            // Table name
            builder.ToTable("WL_MasterMenu");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_WL_MasterMenu_UUID");

            // MenuName
            builder.Property(x => x.MenuName)
                .HasMaxLength(50)
                .IsRequired(false);

            // MenuIcon
            builder.Property(x => x.MenuIcon)
                .HasMaxLength(100)
                .IsRequired(false);

            // MenuLevel
            builder.Property(x => x.MenuLevel)
                .HasColumnType("decimal(18,0)")
                .IsRequired(false);

            // MainParentUUID
            builder.Property(x => x.MainParentUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

           

            // PermissionUUID
            builder.Property(x => x.PermissionUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            // Url
            builder.Property(x => x.Url)
                .HasMaxLength(200)
                .IsRequired(false);

            // IsParent
            builder.Property(x => x.IsParent)
                .IsRequired();

            // Sequence
            builder.Property(x => x.Sequence)
                .HasColumnType("decimal(18,0)")
                .IsRequired(false);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT

            // Query filter for soft delete (optional - apply if you want automatic filtering)
            // builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}