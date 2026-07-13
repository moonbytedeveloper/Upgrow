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
    public class Inquiry_AgentConfiguration : IEntityTypeConfiguration<Inquiry_Agent>
    {
        public void Configure(EntityTypeBuilder<Inquiry_Agent> builder)
        {
            builder.ToTable("Inquiry_Agent");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn();

            builder.Property(x => x.UUID).HasMaxLength(50).IsRequired();
            builder.Property(x => x.FName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PhoneNo).HasMaxLength(50).IsRequired();

            builder.Property(x => x.SalesExperience).IsRequired();
            builder.Property(x => x.IsConvertedToAgent).IsRequired();
            builder.Property(x => x.IsStatusClosed).IsRequired();
            builder.Property(x => x.SalesExperienceDescription)
                .HasMaxLength(500);

            builder.Property(x => x.StateUUID).HasMaxLength(50);
            builder.Property(x => x.CityUUID).HasMaxLength(50);

            builder.Property(x => x.HasExistingClients)
                .HasMaxLength(50);

            builder.Property(x => x.Message)
                .HasMaxLength(1000);

            builder.Property(x => x.Remark)
                .HasMaxLength(1000);
            builder.Property(x => x.ActionTakenBy).HasMaxLength(50);

            // Status - Open(True) and Closed(False)
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
