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
    public class TransactionRzpPGRecordConfiguration
        : IEntityTypeConfiguration<TransactionRzpPGRecord>
    {
        public void Configure(
            EntityTypeBuilder<TransactionRzpPGRecord> builder)
        {
            builder.ToTable(
                "TransactionRzpPGRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.Property(x => x.TransactionUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.TransactionUUID);

            builder.Property(x => x.OrderId)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.OrderId);

            builder.Property(x => x.OrderAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.OrderCreatedAt)
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.OrderStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PaymentId)
                .HasMaxLength(100);

            builder.HasIndex(x => x.PaymentId);

            builder.Property(x => x.PaymentCreatedAt)
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(x => x.Bank)
                .HasMaxLength(100);

            builder.Property(x => x.Vpa)
                .HasMaxLength(200);

            builder.Property(x => x.Wallet)
                .HasMaxLength(100);

            builder.Property(x => x.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Fee)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Tax)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ErrorCode)
                .HasMaxLength(100);

            builder.Property(x => x.ErrorDescription)
                .HasMaxLength(1000);

            builder.Property(x => x.IsCurrent)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
