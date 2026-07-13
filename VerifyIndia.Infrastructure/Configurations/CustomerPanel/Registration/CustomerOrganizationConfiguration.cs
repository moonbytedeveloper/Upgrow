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
    public sealed class CustomerOrganizationConfiguration
        : IEntityTypeConfiguration<CustomerOrganization>
    {
        public void Configure(
            EntityTypeBuilder<CustomerOrganization> builder)
        {
            builder.ToTable("CustomerOrganization");

            builder.HasKey(x => x.Id);

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

            builder.HasIndex(x => x.CustomerUUID)
                .IsUnique();

            builder.Property(x => x.BusinessTypeUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.BusinessName)
                .HasMaxLength(500);

            builder.Property(x => x.BusinessRegistrationNumber)
                .HasMaxLength(100);

            builder.Property(x => x.GSTIN)
                .HasMaxLength(20);

            builder.Property(x => x.PAN)
                .HasMaxLength(20);

            builder.Property(x => x.SelectedDirectorName)
                .HasMaxLength(250);

            builder.Property(x => x.SelectedDirectorDIN)
                .HasMaxLength(20);

            builder.Property(x => x.VerificationStatus)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.HasOne<Master_Customer>()
                .WithOne()
                .HasForeignKey<CustomerOrganization>(
                    x => x.CustomerUUID)
                .HasPrincipalKey<Master_Customer>(
                    x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Master_BusinessType>()
                .WithMany()
                .HasForeignKey(
                    x => x.BusinessTypeUUID)
                .HasPrincipalKey(
                    x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
