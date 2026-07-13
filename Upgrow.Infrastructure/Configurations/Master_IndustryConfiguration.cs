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
    public class Master_IndustryConfiguration : IEntityTypeConfiguration<Master_Industry>
    {
        public void Configure(EntityTypeBuilder<Master_Industry> builder)
        {
            // Table name
            builder.ToTable("Master_Industry");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Master_Industry_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Master_Industry_Title");

            // BlogCategoryUUID - Foreign key reference
          

            // Blog Image
            builder.Property(x => x.Image)
                .HasColumnType("nvarchar(200)")
                .IsRequired(true);

            // Banner Image
            builder.Property(x => x.Image)
                .HasColumnType("nvarchar(200)")
                .IsRequired(true);

            // Short Description
            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(500)")
                .IsRequired(false);

          
            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
               .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT

        }
    }
}
   
