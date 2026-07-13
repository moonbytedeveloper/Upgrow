using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Inquiry;

namespace Upgrow.Infrastructure.Configurations.Inquiry
{
    public class Inquiry_WhiteLabelConfiguration : IEntityTypeConfiguration<Inquiry_WhiteLabel>
    {
        public void Configure(EntityTypeBuilder<Inquiry_WhiteLabel> builder)
        {
            builder.ToTable("Inquiry_WhiteLabel");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn();

            builder.Property(x => x.UUID).HasMaxLength(50).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EmailId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PhoneNo).HasMaxLength(50).IsRequired();

            builder.Property(x => x.CompanyName).HasMaxLength(50);
            builder.Property(x => x.CompanyWebsite).HasMaxLength(50);

            builder.Property(x => x.StateUUID).HasMaxLength(50);
            builder.Property(x => x.CityUUID).HasMaxLength(50);

            builder.Property(x => x.BusinessType).HasMaxLength(50);

            builder.Property(x => x.PreferredDomainName).HasMaxLength(50);

            builder.Property(x => x.Message).HasMaxLength(1000);

            builder.Property(x => x.Remark).HasMaxLength(1000);
            builder.Property(x => x.ActionTakenBy).HasMaxLength(50);

            // Status - Open(True) and Closed(False)
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
