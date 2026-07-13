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
    public class CustomerDebitLedgerConfiguration :
        IEntityTypeConfiguration<CustomerDebitLedger>
    {
        public void Configure(
            EntityTypeBuilder<CustomerDebitLedger> builder)
        {
            builder.ToTable("CustomerDebitLedger");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
    .HasColumnType("decimal(18,0)")
    .UseIdentityColumn()
    .IsRequired();

            builder.Property(x => x.UUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CustomerUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DebitType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CreditsDebited)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.BalanceBefore)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.BalanceAfter)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.CustomerUUID);

            builder.HasIndex(x => x.TransactionUUID);

            builder.Property(x => x.IsActive)
    .IsRequired()
    .HasDefaultValue(true)
    .ValueGeneratedNever();
        }
    }
}
