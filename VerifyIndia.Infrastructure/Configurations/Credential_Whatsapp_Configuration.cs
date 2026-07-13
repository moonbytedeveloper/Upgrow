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
    public class Credential_Whatsapp_Configuration : IEntityTypeConfiguration<Credential_Whatsapp>
    {
        public void Configure(EntityTypeBuilder<Credential_Whatsapp> builder)
        {
            // Table name
            builder.ToTable("Credential_Whatsapp");

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
                .HasDatabaseName("IX_Credential_Whatsapp_UUID");

            // APIKey - WhatsApp API Key
            builder.Property(x => x.APIKey)
                .HasMaxLength(50)
                .IsRequired();

            // Secret - WhatsApp Secret Key
            builder.Property(x => x.AccessToken)
                .HasMaxLength(4000)
                .IsRequired();

            // MobileNo - WhatsApp Mobile Number
            builder.Property(x => x.MobileNo)
                .HasMaxLength(20)
                .IsRequired();

            // SenderName - Display name for WhatsApp sender
            builder.Property(x => x.SenderName)
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