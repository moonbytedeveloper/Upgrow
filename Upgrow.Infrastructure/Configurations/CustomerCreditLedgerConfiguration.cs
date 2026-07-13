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
    public class CustomerCreditLedgerConfiguration :
        IEntityTypeConfiguration<CustomerCreditLedger>
    {
        public void Configure(
            EntityTypeBuilder<CustomerCreditLedger> builder)
        {
            builder.ToTable("CustomerCreditLedger");

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
                .HasMaxLength(50);

            builder.Property(x => x.CreditType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CreditsAdded)
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

            builder.Property(x => x.IsActive)
    .IsRequired()
    .HasDefaultValue(true)
    .ValueGeneratedNever();
        }
    }
}
