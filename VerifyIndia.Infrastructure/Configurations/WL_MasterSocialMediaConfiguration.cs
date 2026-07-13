using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class WL_MasterSocialMediaConfiguration : IEntityTypeConfiguration<WL_MasterSocialMedia>
    {
        public void Configure(EntityTypeBuilder<WL_MasterSocialMedia> builder)
        {
            builder.ToTable("WL_MasterSocialMedia");

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
                .HasDatabaseName("IX_WL_MasterSocialMedia_UUID");

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

   
