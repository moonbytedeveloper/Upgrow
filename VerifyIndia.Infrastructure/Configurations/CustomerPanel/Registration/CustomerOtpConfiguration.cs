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
    public sealed class CustomerOtpConfiguration
        : IEntityTypeConfiguration<CustomerOtp>
    {
        public void Configure(
            EntityTypeBuilder<CustomerOtp> builder)
        {
            builder.ToTable("CustomerOtp");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.OtpHash)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ExpiresAtUtc)
                .IsRequired();

            builder.Property(x => x.VerifyAttemptCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.CustomerUUID);
        }
    }
}
