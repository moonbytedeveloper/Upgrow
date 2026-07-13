using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Infrastructure.Configurations.CustomerPanel.Registration
{
    public sealed class MasterBusinessTypeConfiguration
        : IEntityTypeConfiguration<Master_BusinessType>
    {
        public void Configure(
            EntityTypeBuilder<Master_BusinessType> builder)
        {
            builder.ToTable("Master_BusinessType");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)");

            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.DisplayOrder)
                .HasColumnType("numeric(18,0)")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

        }
    }
}
