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
    public class CartDetailConfiguration
    : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(
            EntityTypeBuilder<CartDetail> builder)
        {
            builder.ToTable("CartDetail");

            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.Property(x => x.ApiUUID)
                .HasMaxLength(50);

            builder.Property(x => x.CartUUID)
                .HasMaxLength(50);

            builder.Property(x => x.PricingUUID)
                .HasMaxLength(50);

            builder.Property(x => x.ApiCharge)
                .HasPrecision(18, 2);

            builder.Property(x => x.ReqPayload)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
