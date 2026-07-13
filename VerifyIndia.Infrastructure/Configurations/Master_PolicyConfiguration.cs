using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class Master_PolicyConfiguration : IEntityTypeConfiguration<Master_Policy>
    {
        public void Configure(EntityTypeBuilder<Master_Policy> builder)
        {
            builder.ToTable("Master_Policy");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PolicyContent)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.ContentHash)
               .HasMaxLength(64)
               .IsRequired(false);

            builder.Property(x => x.Version)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.SequenceNo)
                .IsRequired();
        }
    }
}
