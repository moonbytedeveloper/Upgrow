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
    public class MasterDosDontsDocumentConfiguration
        : IEntityTypeConfiguration<MasterDosDontsDocument>
    {
        public void Configure(
            EntityTypeBuilder<MasterDosDontsDocument> builder)
        {
            builder.ToTable(
                "Master_DosDontsDocument");

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
                    x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(
                    x => x.VersionNo)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(
                    x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(
                    x => x.PublishedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.CreatedAt)
                .HasColumnType(
                    "datetimeoffset")
                .IsRequired();

            builder.Property(
                    x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(
                x => x.IsActive);

            builder.HasIndex(
                x => x.Status);

            builder.HasMany(x => x.DosDontsItems)
       .WithOne(x => x.Document)
       .HasForeignKey(x => x.DocumentUUID)
       .HasPrincipalKey(x => x.UUID)
       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
