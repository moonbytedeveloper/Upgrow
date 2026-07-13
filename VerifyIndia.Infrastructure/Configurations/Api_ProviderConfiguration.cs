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
    public class Api_ProviderConfiguration : IEntityTypeConfiguration<Api_Provider>
    {
        public void Configure(EntityTypeBuilder<Api_Provider> builder)
        {
            // Table name
            builder.ToTable("Api_Provider");

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
                .HasDatabaseName("IX_Api_Provider_UUID");

            // Title
            builder.Property(x => x.ProviderName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(x => x.ProviderName)
                .HasDatabaseName("IX_Api_Provider_ProviderName");
            // Code - System identifier (auto-generated)
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(x => x.Code)
                .HasDatabaseName("IX_Api_Provider_Code");

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
    
   
