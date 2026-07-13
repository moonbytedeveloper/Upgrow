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
    public class Website_IndustryCasesConfiguration : IEntityTypeConfiguration<Website_IndustryCases>
    {
        public void Configure(EntityTypeBuilder<Website_IndustryCases> builder)
        {
            // Table name
            builder.ToTable("Website_IndustryCases");

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
                .HasDatabaseName("IX_Website_IndustryCases_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Website_IndustryCases_Title");

            // ShortTitle
            builder.Property(x => x.Icon)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.IndustryUUID)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.Sequence)
                .IsRequired(true);

            builder.HasIndex(x => x.Sequence)
                .HasDatabaseName("IX_Website_IndustryCases_Sequence");




            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}
