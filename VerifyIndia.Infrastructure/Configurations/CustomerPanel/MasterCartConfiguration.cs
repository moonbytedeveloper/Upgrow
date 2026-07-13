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
    public class MasterCartConfiguration
    : IEntityTypeConfiguration<Master_Cart>
    {
        public void Configure(
            EntityTypeBuilder<Master_Cart> builder)
        {
            builder.ToTable("Master_Cart");

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

            builder.Property(x => x.CartNo)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.CartNo)
                .IsUnique();

            builder.Property(x => x.AuthFor)
                .HasMaxLength(20);

            builder.Property(x => x.VerifierUUID)
                .HasMaxLength(50);

            builder.Property(x => x.SellerTenantUUID)
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .HasMaxLength(20);

            builder.Property(x => x.BaseCreditsTotal)
                .HasPrecision(18, 2);

            builder.Property(x => x.ConsentCreditsTotal)
                .HasPrecision(18, 2);

            builder.Property(x => x.PayableBaseAmount)
                .HasPrecision(18, 2);

            builder.HasMany(x => x.CartDetails)
                .WithOne(x => x.Cart)
                .HasPrincipalKey(x => x.UUID)
                .HasForeignKey(x => x.CartUUID);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
