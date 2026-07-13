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
    public class MasterDosDontsConfiguration
        : IEntityTypeConfiguration<Master_DosDonts>
    {
        public void Configure(
            EntityTypeBuilder<Master_DosDonts> builder)
        {
            builder.ToTable(
                "Master_DosDonts");

            builder.HasKey(
                x => x.Id);

            builder.Property(
                    x => x.Id)
                .HasColumnType(
                    "numeric(18,0)");

            builder.Property(
                    x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(
                    x => x.UUID)
                .IsUnique();

            builder.Property(
                    x => x.DocumentUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                    x => x.Message)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(
                    x => x.IsDos)
                .IsRequired();

            builder.Property(
                    x => x.SequenceNo)
                .HasColumnType(
                    "numeric(18,0)");

            builder.Property(
                    x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(
                x => new
                {
                    x.DocumentUUID,
                    x.SequenceNo
                });

            builder.HasOne(
                    x => x.Document)
                .WithMany(
                    x => x.DosDontsItems)
                .HasForeignKey(
                    x => x.DocumentUUID)
                .HasPrincipalKey(
                    x => x.UUID)
                .OnDelete(
                    DeleteBehavior.Restrict);
        }
    }
}