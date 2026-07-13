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
    public class MasterSMSCredentialConfiguration
        : IEntityTypeConfiguration<MasterSMSCredential>
    {
        public void Configure(
            EntityTypeBuilder<MasterSMSCredential> builder)
        {
            builder.ToTable(
                "Master_SMSCredential");

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
                    x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(
                    x => x.BaseUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(
                    x => x.AuthKey)
                .HasMaxLength(500)
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
        }
    }
}
