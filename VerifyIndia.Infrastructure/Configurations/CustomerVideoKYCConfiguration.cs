using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class CustomerVideoKYCConfiguration : IEntityTypeConfiguration<CustomerVideoKYC>
    {
        public void Configure(EntityTypeBuilder<CustomerVideoKYC> builder)
        {
            builder.ToTable("CustomerVideoKYC");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.VideoFileUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.KycText)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.TimeStamp)
                .HasColumnType("datetimeoffset(7)")
                .IsRequired();

            builder.Property(x => x.IsKycDone)
                .IsRequired();

            builder.Property(x => x.IsTimeout)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(x => x.OldRecordUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(x => x.CustomerUUID)
                .HasDatabaseName("IX_CustomerVideoKYC_CustomerUUID");
        }
    }
}
