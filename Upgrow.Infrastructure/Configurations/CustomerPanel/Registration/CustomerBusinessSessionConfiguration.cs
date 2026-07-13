using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Infrastructure.Configurations.CustomerPanel.Registration
{
    public sealed class CustomerBusinessSessionConfiguration
        : IEntityTypeConfiguration<CustomerBusinessSession>
    {
        public void Configure(
            EntityTypeBuilder<CustomerBusinessSession> builder)
        {
            builder.ToTable(
                "CustomerBusinessSession");

            builder.HasKey(
                x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerOrganizationUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.RazorpayOrderId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsActive)
    .IsRequired()
    .HasDefaultValue(true)
    .ValueGeneratedNever();

            builder.HasIndex(
                x => x.CustomerUUID);

            builder.HasIndex(
                x => x.RazorpayOrderId);

            builder.HasOne<Master_Customer>()
                .WithMany()
                .HasForeignKey(
                    x => x.CustomerUUID)
                .HasPrincipalKey(
                    x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<CustomerOrganization>()
                .WithMany()
                .HasForeignKey(
                    x => x.CustomerOrganizationUUID)
                .HasPrincipalKey(
                    x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
