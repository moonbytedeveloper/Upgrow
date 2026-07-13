using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL.Master;

namespace Upgrow.Infrastructure.Configurations.WL.Master
{
    public class WL_MenuRolePermissionConfiguration : IEntityTypeConfiguration<WL_MenuRolePermission>
    {
        public void Configure(EntityTypeBuilder<WL_MenuRolePermission> builder)
        {
            // Table name
            builder.ToTable("WL_MenuRolePermission");

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
                .HasDatabaseName("IX_WL_MenuRolePermission_UUID");


            builder.Property(x => x.RoleUUID)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => x.RoleUUID)
                .HasDatabaseName("IX_WL_MenuRolePermission_RoleUUID");

            builder.Property(x => x.PermissionUUID)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => x.PermissionUUID)
                .HasDatabaseName("IX_WL_MenuRolePermission_PermissionUUID");

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
   
   
