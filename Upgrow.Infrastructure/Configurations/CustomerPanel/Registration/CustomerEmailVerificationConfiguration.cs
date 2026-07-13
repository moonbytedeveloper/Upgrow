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
    public class CustomerEmailVerificationConfiguration
        : IEntityTypeConfiguration<CustomerEmailVerification>
    {
        public void Configure(
            EntityTypeBuilder<CustomerEmailVerification> builder)
        {
            builder.ToTable(
                "CustomerEmailVerification");

            builder.HasKey(
                x => x.Id);

            builder.Property(x => x.Id)
                 .HasColumnType("numeric(18,0)")
                 .UseIdentityColumn()
                 .IsRequired();

            builder.Property(
                    x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(
                    x => x.UUID)
                .IsUnique();

            builder.Property(
                    x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                    x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(
                    x => x.OTP)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(
                    x => x.OTPSentAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.OTPExpiresAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.LastResendAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.VerifiedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.FailureReason)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(
                    x => x.CreatedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.UpdatedAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.HasIndex(
                x => new
                {
                    x.CustomerUUID,
                    x.IsActive
                });

            builder.HasIndex(
                x => new
                {
                    x.Email,
                    x.IsActive
                });
        }
    }
}
