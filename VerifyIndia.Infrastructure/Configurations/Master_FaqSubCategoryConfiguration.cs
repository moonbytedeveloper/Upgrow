using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Master_FaqSubCategoryConfiguration : IEntityTypeConfiguration<Master_FAQSubCategory>
    {
        public void Configure(EntityTypeBuilder<Master_FAQSubCategory> builder)
        {
            // Table name
            builder.ToTable("Master_FAQSubCategory");

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
                .HasDatabaseName("IX_Master_FAQSubCategory_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.Image)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Master_FAQSubCategory_Title");


            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}

   

