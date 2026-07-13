using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class ApiXCategoryConfiguration : IEntityTypeConfiguration<ApiXCategory>
    {
        public void Configure(EntityTypeBuilder<ApiXCategory> builder)
        {
            // Table name
            builder.ToTable("ApiXCategory");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasDatabaseName("IX_ApiXCategory_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_ApiXCategory_Title");

            // Description
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            // SequenceNo
            builder.Property(x => x.SequenceNo)
                .IsRequired();

            // Icon
            builder.Property(x => x.Icon)
                .HasMaxLength(50)
                .IsRequired(false);

            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}