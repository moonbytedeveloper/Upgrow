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
    public class WL_MasterBannerConfiguration : IEntityTypeConfiguration<WL_MasterBanner>
    {
        public void Configure(EntityTypeBuilder<WL_MasterBanner> builder)
        {
            builder.ToTable("WL_MasterBanner");

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
                .HasDatabaseName("IX_WL_MasterBanner_UUID");

            builder.Property(x => x.MainTitle)
                .HasColumnName("MainTitle")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.SubTitle)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.OptionalTitle)
               .HasMaxLength(50)
               .IsRequired(false);

            builder.Property(x => x.ButtonText)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.ButtonURL)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.BannerImage)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.SequenceNo)
                .HasColumnType("decimal(18,0)")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();


        }
    }
}
