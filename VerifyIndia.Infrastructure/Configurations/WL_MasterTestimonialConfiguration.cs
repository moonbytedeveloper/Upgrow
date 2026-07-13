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
    public class WL_MasterTestimonialConfiguration : IEntityTypeConfiguration<WL_MasterTestimonial>
    {
        public void Configure(EntityTypeBuilder<WL_MasterTestimonial> builder)
        {
            // Table name
            builder.ToTable("WL_MasterTestimonial");

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
                .HasDatabaseName("IX_WL_MasterTestimonial_UUID");

            builder.Property(x => x.CustomerName)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(x => x.CompanyName)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.FilePath)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SequenceNo)
                .HasColumnType("decimal(18,0)")
                .IsRequired();

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
