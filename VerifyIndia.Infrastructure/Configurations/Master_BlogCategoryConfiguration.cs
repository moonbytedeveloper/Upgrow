using VerifyIndia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Master_BlogCategoryConfiguration : IEntityTypeConfiguration<Master_BlogCategory>
    {
        public void Configure(EntityTypeBuilder<Master_BlogCategory> builder)
        {
            // Table name
            builder.ToTable("Master_BlogCategory");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique external identifier
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Master_Banner_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Master_Banner_Title");

            // Image URL
            builder.Property(x => x.ImageUrl)
                .HasColumnType("nvarchar(200)")
                .IsRequired(false);

            // Description
            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(MAX)")
                .IsRequired(false);

            // IsActive
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
        }
    }
}