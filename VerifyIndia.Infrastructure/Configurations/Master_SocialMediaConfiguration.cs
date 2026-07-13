using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Master_SocialMediaConfiguration : IEntityTypeConfiguration<Master_SocialMedia>
    {
        public void Configure(EntityTypeBuilder<Master_SocialMedia> builder)
        {
            builder.ToTable("Master_SocialMedia");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Master_SocialMedia_UUID");

            builder.Property(x => x.PlatformName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ProfileURL)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.IconURL)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DisplayOrder)
                .HasColumnType("decimal(18,0)")
                .IsRequired();
            
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
