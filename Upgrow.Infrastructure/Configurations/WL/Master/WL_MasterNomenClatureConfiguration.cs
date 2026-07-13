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
    internal class WL_MasterNomenClatureConfiguration : IEntityTypeConfiguration<WL_MasterNomenClature>
    {
        public void Configure(EntityTypeBuilder<WL_MasterNomenClature> builder)
        {
            // Table name
            builder.ToTable("WL_MasterNomenClature");

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
                .HasDatabaseName("IX_WL_MasterNomenClature_UUID");

            // ModuleKey - Unique module identifier
            builder.Property(x => x.ModuleKey)
                .HasMaxLength(80)
                .IsRequired();

            builder.HasIndex(x => x.ModuleKey)
                .IsUnique()
                .HasDatabaseName("IX_WL_MasterNomenClature_ModuleKey");

            // IsIncludeYear - Boolean flag
            builder.Property(x => x.IsIncludeYear)
                .IsRequired()
                .HasDefaultValue(false)
                .ValueGeneratedNever();

            // Prefix - Optional prefix for nomenclature
            builder.Property(x => x.Prefix)
                .HasMaxLength(20)
                .IsRequired(false);

            // StartNo - Starting number
            builder.Property(x => x.StartNo)
                .HasColumnType("numeric(18,0)")
                .IsRequired();

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
    