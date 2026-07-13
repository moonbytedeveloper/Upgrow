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
    public class NotificationSoundConfiguration : IEntityTypeConfiguration<NotificationSound>
    {
        public void Configure(EntityTypeBuilder<NotificationSound> builder)
        {
            // Table name
            builder.ToTable("NotificationSound");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.UUID)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasDatabaseName("IX_NotificationSound_UUID");

            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
