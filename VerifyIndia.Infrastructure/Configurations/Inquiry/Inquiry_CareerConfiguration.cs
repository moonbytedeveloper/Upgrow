using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Inquiry;

namespace VerifyIndia.Infrastructure.Configurations.Inquiry
{
    public class Inquiry_CareerConfiguration : IEntityTypeConfiguration<Inquiry_Career>
    {
        public void Configure(EntityTypeBuilder<Inquiry_Career> builder)
        {
            builder.ToTable("Inquiry_Career");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn();

            builder.Property(x => x.UUID).HasMaxLength(50).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PhoneNo).HasMaxLength(50).IsRequired();

            builder.Property(x => x.JobPosition).HasMaxLength(50);
            builder.Property(x => x.Experience).HasMaxLength(50);
            builder.Property(x => x.Qualification).HasMaxLength(50);

            builder.Property(x => x.Message).HasMaxLength(1000);

            builder.Property(x => x.Remark).HasMaxLength(1000);

            builder.Property(x => x.Resume).HasMaxLength(100);
            builder.Property(x => x.ActionTakenBy).HasMaxLength(50);

            // Status - Open(True) and Closed(False)
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
