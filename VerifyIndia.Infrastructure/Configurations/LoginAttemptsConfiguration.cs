using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class LoginAttemptsConfiguration : IEntityTypeConfiguration<LoginAttempts>
    {
        public void Configure(EntityTypeBuilder<LoginAttempts> builder)
        {
            builder.ToTable("LoginAttempts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.Payload);
            builder.Property(x => x.PreviousHash);
            builder.Property(x => x.CurrentHash);
            builder.Property(x => x.DigitalSignature);
        }
    }
}