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
    public class Master_FinancialYearConfiguration : IEntityTypeConfiguration<Master_FinancialYear>
    {
        public void Configure(EntityTypeBuilder<Master_FinancialYear> builder)
        {
            // Table name
            builder.ToTable("Master_FinancialYear");

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
                .HasDatabaseName("IX_Master_FinancialYear_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Master_City_Title");

           
            builder.Property(x => x.StartDate)
                .HasMaxLength(20)
                .IsRequired(false);
            builder.Property(x => x.EndDate)
               .HasMaxLength(20)
               .IsRequired(false);

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


