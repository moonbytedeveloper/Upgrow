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
    public class TrustedPartnersConfiguration : IEntityTypeConfiguration<TrustedPartners>
    {
        public void Configure(EntityTypeBuilder<TrustedPartners> builder)
        {
            // Table name
            builder.ToTable("TrustedPartners");

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
                .HasDatabaseName("IX_Clients_UUID");
            // Name - Partner name
            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(150)")
                .IsRequired();

            // IconImage - Partner icon image path
            builder.Property(x => x.IconImage)
                .HasColumnType("nvarchar(200)");

         
            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
