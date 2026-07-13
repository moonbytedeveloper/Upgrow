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
    public class TransactionDetailConfiguration :
        IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(
            EntityTypeBuilder<TransactionDetail> builder)
        {
            builder.ToTable("TransactionDetail");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ApiUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PricingUUID)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ApiCharge)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ReqPl);

            builder.Property(x => x.RecordDatetime);

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.TransactionUUID);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.HasOne(x => x.Transaction)
    .WithMany(x => x.TransactionDetails)
    .HasForeignKey(x => x.TransactionUUID)
    .HasPrincipalKey(x => x.UUID)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
