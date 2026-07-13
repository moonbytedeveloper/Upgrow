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
    public class MasterSMSTemplateConfiguration
        : IEntityTypeConfiguration<MasterSMSTemplate>
    {
        public void Configure(
            EntityTypeBuilder<MasterSMSTemplate> builder)
        {
            builder.ToTable(
                "Master_SMSTemplate");

            builder.HasKey(
                x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(
                    x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(
                    x => x.UUID)
                .IsUnique();

            builder.Property(
                    x => x.EventCode)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(
                    x => x.SMSCredentialUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                    x => x.ProviderTemplateId)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(
                    x => x.CreatedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.UpdatedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.HasIndex(
                x => x.EventCode);

            builder.HasIndex(
                x => x.SMSCredentialUUID);
        }
    }
}
