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
    public class TransactionConfiguration :
        IEntityTypeConfiguration<Transaction>
    {
        public void Configure(
            EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionNo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CartUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.AuthFor)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.VerifierUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PaymentMode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.PaymentFrom)
                .HasMaxLength(50);

            builder.Property(x => x.PaymentStatus)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.BaseCreditsTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ConsentCreditsTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PayableBaseAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IGST)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CGST)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.SGST)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.GST)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPayableAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedAt);

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.TransactionNo)
                .IsUnique();

            builder.HasMany(x => x.TransactionDetails)
                .WithOne(x => x.Transaction)
                .HasForeignKey(x => x.TransactionUUID)
                .HasPrincipalKey(x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransactionConsent)
                .WithOne(x => x.Transaction)
                .HasForeignKey<TransactionConsent>(
                    x => x.TransactionUUID)
                .HasPrincipalKey<Transaction>(
                    x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
