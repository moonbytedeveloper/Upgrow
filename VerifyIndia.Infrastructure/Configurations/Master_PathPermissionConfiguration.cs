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
    public class Master_PathPermissionConfiguration : IEntityTypeConfiguration<Master_PathPermission>
    {
        public void Configure(EntityTypeBuilder<Master_PathPermission> builder)
        {
            builder.ToTable("Master_PathPermission");

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
                .HasDatabaseName("IX_Master_ManagePath_UUID");

            builder.Property(x => x.PathUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PermissionUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
               .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}
   
