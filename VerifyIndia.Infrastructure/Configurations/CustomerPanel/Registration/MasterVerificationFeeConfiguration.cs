using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Infrastructure.Configurations.CustomerPanel.Registration
{
    public sealed class MasterVerificationFeeConfiguration
        : IEntityTypeConfiguration<Master_VerificationFee>
    {
        public void Configure(
            EntityTypeBuilder<Master_VerificationFee> builder)
        {
            builder.ToTable(
                "Master_VerificationFee");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique();
        }
    }
}
