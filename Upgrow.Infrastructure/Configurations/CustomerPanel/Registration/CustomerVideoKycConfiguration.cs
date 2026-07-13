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
    public class CustomerVideoKycConfiguration
        : IEntityTypeConfiguration<CustomerVideoKyc>
    {
        public void Configure(
            EntityTypeBuilder<CustomerVideoKyc> builder)
        {
            builder.ToTable(
                "CustomerVideoKyc");

            builder.HasKey(
                x => x.Id);

            builder.Property(
                x => x.Id)
                .HasColumnType(
                    "numeric(18,0)")
                .UseIdentityColumn();

            builder.Property(
                x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                x => x.VideoUrl)
                .HasMaxLength(500);

            builder.Property(
                x => x.ChallengeText)
                .HasMaxLength(1000);

            builder.Property(
    x => x.SpokenText)
    .HasMaxLength(1000);

            builder.Property(
                x => x.SpeechMatchPercentage)
                .HasColumnType(
                    "decimal(18,2)");

            builder.Property(
                x => x.FailureReason)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.HasIndex(
                x => x.CustomerUUID);
        }
    }
}
