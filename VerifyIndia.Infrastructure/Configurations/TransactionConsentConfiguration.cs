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
    public class TransactionConsentConfiguration :
        IEntityTypeConfiguration<TransactionConsent>
    {
        public void Configure(
            EntityTypeBuilder<TransactionConsent> builder)
        {
            builder.ToTable("TransactionConsent");

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

            builder.Property(x => x.ConsentDocNo)
                .HasMaxLength(500);

            builder.Property(x => x.ConsentMobileNo)
                .HasMaxLength(20);

            builder.Property(x => x.ConsentToken)
                .HasMaxLength(100);

            builder.Property(x => x.AadharReqId)
                .HasMaxLength(100);

            builder.Property(x => x.ConsentStatus)
                .HasMaxLength(20);

            builder.Property(x => x.ConsentSubmitterName)
                .HasMaxLength(200);

            builder.Property(x => x.ConsentSubmitterLat)
                .HasMaxLength(50);

            builder.Property(x => x.ConsentSubmitterLong)
                .HasMaxLength(50);

            builder.Property(x => x.ConsentSubmitterCurrentCity)
                .HasMaxLength(200);

            builder.Property(x => x.ConsentSubmitterDevice)
                .HasMaxLength(500);

            builder.Property(x => x.ConsentSubmitterIp)
                .HasMaxLength(100);

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.TransactionUUID)
                .IsUnique();

            builder.HasIndex(x => x.ConsentToken);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
