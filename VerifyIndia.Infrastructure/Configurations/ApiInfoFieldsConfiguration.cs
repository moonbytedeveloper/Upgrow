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
    public class ApiInfoFieldsConfiguration : IEntityTypeConfiguration<ApiInfoFields>
    {
        public void Configure(EntityTypeBuilder<ApiInfoFields> builder)
        {
            // Table name
            builder.ToTable("ApiInfoFields");

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
                .HasDatabaseName("IX_ApiInfoFields_UUID");
            // ShortTitle
            builder.Property(x => x.InfoSectionUUID)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);
 

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
            
            builder.Property(x => x.Sequence)
                .IsRequired();
                 

            // Query filter for soft delete (optional - apply if you want automatic filtering)
            // builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}
    
   