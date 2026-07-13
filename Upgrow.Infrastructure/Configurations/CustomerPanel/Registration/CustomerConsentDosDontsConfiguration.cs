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
    public class CustomerConsentDosDontsConfiguration
        : IEntityTypeConfiguration<CustomerConsentDosDonts>
    {
        public void Configure(
            EntityTypeBuilder<CustomerConsentDosDonts> builder)
        {
            builder.ToTable(
                "CustomerConsent_DosDonts");

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
                    x => x.DocumentUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                    x => x.ConsentGiven)
                .IsRequired();

            builder.Property(
                    x => x.ConsentGivenAt)
                .HasColumnType(
                    "datetimeoffset");

            builder.Property(
                    x => x.IPAddress)
                .HasMaxLength(50);

            builder.Property(
                    x => x.Platform)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.HasIndex(
                x => new
                {
                    x.CustomerUUID,
                    x.IsActive
                });

            builder.HasIndex(
                x => x.DocumentUUID);

            // Uncomment if you have the corresponding entities

            /*
            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerUUID)
                .HasPrincipalKey(x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<MasterDosDontsDocument>()
                .WithMany()
                .HasForeignKey(x => x.DocumentUUID)
                .HasPrincipalKey(x => x.UUID)
                .OnDelete(DeleteBehavior.Restrict);
            */


        }
    }
}
