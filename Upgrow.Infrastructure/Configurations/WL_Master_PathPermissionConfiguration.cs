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
    public class WL_Master_PathPermissionConfiguration : IEntityTypeConfiguration<WL_Master_PathPermission>
    {
        public void Configure(EntityTypeBuilder<WL_Master_PathPermission> builder)
        {
            builder.ToTable("WL_Master_PathPermission");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_WL_Master_PathPermission_UUID");

            builder.Property(x => x.PathUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PermissionUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
