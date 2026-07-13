using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Infrastructure.Configurations.CustomerPanel.Registration
{
    public class CustomerRegDocumentConfiguration
        : IEntityTypeConfiguration<CustomerRegDocument>
    {
        public void Configure(
            EntityTypeBuilder<CustomerRegDocument> builder)
        {
            builder.ToTable(
                "CustomerRegDocument");

            builder.HasKey(
                x => x.Id);

            builder.Property(
                x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(
                x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                x => x.RecordType)
                .HasMaxLength(50);

            builder.Property(
                x => x.RecordNo)
                .HasMaxLength(50);

            builder.Property(
                x => x.RecordCategory)
                .HasMaxLength(50);

            builder.Property(
                x => x.VerifiedFrom)
                .HasMaxLength(10);

            builder.Property(
                x => x.IpAddress)
                .HasMaxLength(50);

            builder.Property(
                x => x.Latitude)
                .HasMaxLength(50);

            builder.Property(
                x => x.Longitude)
                .HasMaxLength(50);

            builder.Property(
                x => x.City)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
