using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;

namespace Upgrow.Infrastructure.Configurations
{
    public class Service_CategoryConfiguration : IEntityTypeConfiguration<Service_Category>
    {
        public void Configure(EntityTypeBuilder<Service_Category> builder)
        {
            // Table name
            builder.ToTable("Service_Category");

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
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasDatabaseName("IX_Service_Category_UUID");

            // CategoryName
            builder.Property(x => x.CategoryName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasIndex(x => x.CategoryName)
                .HasDatabaseName("IX_Service_Category_CategoryName");

            // IconImage
            builder.Property(x => x.IconImage)
                .HasMaxLength(200)
                .IsRequired(false);

            // Description
            builder.Property(x => x.Description)
                .HasMaxLength(255)
                .IsRequired(false);

            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
   
