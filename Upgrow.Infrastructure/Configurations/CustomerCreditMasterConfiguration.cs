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
    public class CustomerCreditMasterConfiguration :
        IEntityTypeConfiguration<CustomerCreditMaster>
    {
        public void Configure(
            EntityTypeBuilder<CustomerCreditMaster> builder)
        {
            builder.ToTable("CustomerCreditMaster");

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

            builder.Property(x => x.OpeningBalance)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CurrentBalance)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalCreditsPurchased)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalCreditsConsumed)
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.HasIndex(x => x.CustomerUUID)
                .IsUnique();

            builder.Property(x => x.IsActive)
    .IsRequired()
    .HasDefaultValue(true)
    .ValueGeneratedNever();
        }
    }
}
