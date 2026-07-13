using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations.CustomerPanel
{
    public class TransactionBridgeLogConfiguration
    : IEntityTypeConfiguration<TransactionBridgeLog>
    {
        public void Configure(
            EntityTypeBuilder<TransactionBridgeLog> builder)
        {
            builder.ToTable("TransactionBridgeLog");

            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TransactionUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TransactionDetailUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.VerificationCode)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ProviderCode)
                .HasMaxLength(50);

            builder.Property(x => x.ProviderReferenceNo)
                .HasMaxLength(100);

            builder.Property(x => x.PrimaryIdentifier)
                .HasMaxLength(200);

            builder.Property(x => x.ResponseMessage)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.HasIndex(x =>
    x.TransactionUUID);

            builder.HasIndex(x =>
                x.TransactionDetailUUID);

            builder.HasIndex(x =>
                new
                {
                    x.TransactionDetailUUID,
                    x.AttemptNo
                });

            builder.HasOne(x => x.Transaction)
    .WithMany()
    .HasForeignKey(x => x.TransactionUUID)
    .HasPrincipalKey(x => x.UUID)
    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransactionDetail)
                .WithMany()
                .HasForeignKey(x => x.TransactionDetailUUID)
                .HasPrincipalKey(x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
