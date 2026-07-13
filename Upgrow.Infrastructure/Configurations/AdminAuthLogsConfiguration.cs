using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class AdminAuthLogsConfiguration : IEntityTypeConfiguration<AdminAuthLogs>
    {
        public void Configure(EntityTypeBuilder<AdminAuthLogs> builder)
        {
            builder.ToTable("AdminAuthLogs");

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