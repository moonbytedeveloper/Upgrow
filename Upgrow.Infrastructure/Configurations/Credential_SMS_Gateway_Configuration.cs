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
    public class Credential_SMS_Gateway_Configuration : IEntityTypeConfiguration<Credential_SMS_Gateway>
    {
        public void Configure(EntityTypeBuilder<Credential_SMS_Gateway> builder)
        {
            // Table name
            builder.ToTable("Credential_SMS_Gateway");

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
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Credential_SMS_Gateway_UUID");

            // APIKey - SMS Gateway API Key
            builder.Property(x => x.APIKey)
                .HasMaxLength(50)
                .IsRequired();

            // SenderId - SMS Sender ID
            builder.Property(x => x.SenderId)
                .HasMaxLength(50)
                .IsRequired();

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}