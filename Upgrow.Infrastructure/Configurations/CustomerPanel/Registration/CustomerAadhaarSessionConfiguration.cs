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
    public sealed class CustomerAadhaarSessionConfiguration
        : IEntityTypeConfiguration<CustomerAadhaarSession>
    {
        public void Configure(
            EntityTypeBuilder<CustomerAadhaarSession> builder)
        {
            builder.ToTable(
                "CustomerAadhaarSession");

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
                x => x.AadhaarNumberEncrypted)
                .HasColumnType("nvarchar(max)");

            builder.Property(
                x => x.ClientId)
                .HasMaxLength(100);

            builder.Property(
                x => x.RefId)
                .HasMaxLength(100);

            builder.Property(
                x => x.RazorpayOrderId)
                .HasMaxLength(100);

            builder.Property(
                x => x.RazorpayPaymentId)
                .HasMaxLength(100);

            builder.Property(
                x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(
                x => x.FailureReason)
                .HasMaxLength(500);

            builder.Property(
                x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(
                x => x.UUID)
                .IsUnique();

            builder.HasIndex(
                x => x.CustomerUUID);
        }
    }
}
