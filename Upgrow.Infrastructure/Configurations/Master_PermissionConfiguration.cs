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
    public class Master_PermissionConfiguration : IEntityTypeConfiguration<Master_Permission>
    {
        public void Configure(EntityTypeBuilder<Master_Permission> builder)
        {
            // Table name
            builder.ToTable("Master_Permission");

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
                .HasDatabaseName("IX_Master_Permission_UUID");
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_Permission_Name");

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.PermissionGroupUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.PermissionGroupUUID)
                .HasDatabaseName("IX_Permission_PermissionGroupUUID");
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
