using VerifyIndia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Master_BlogConfiguration : IEntityTypeConfiguration<Master_Blog>
    {
        public void Configure(EntityTypeBuilder<Master_Blog> builder)
        {
            // Table name
            builder.ToTable("Master_Blog");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Master_Blog_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Master_Blog_Title");

            // BlogCategoryUUID - Foreign key reference
            builder.Property(x => x.BlogCategoryUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.BlogCategoryUUID)
                .HasDatabaseName("IX_Master_Blog_BlogCategoryUUID");

            // Blog Image
            builder.Property(x => x.BlogImageUrl)
                .HasColumnType("nvarchar(200)")
                .IsRequired(true);

            // Banner Image
            builder.Property(x => x.BannerImageUrl)
                .HasColumnType("nvarchar(200)")
                .IsRequired(true);

            // Short Description
            builder.Property(x => x.ShortDescription)
                .HasColumnType("nvarchar(500)")
                .IsRequired(false);

            // Blog Date
            builder.Property(x => x.BlogDate)
                .HasColumnType("datetime")
                .IsRequired(false);

            builder.HasIndex(x => x.BlogDate)
                .HasDatabaseName("IX_Master_Blog_BlogDate");

            // Full Description
            builder.Property(x => x.FullDescription)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
               .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT

        }
    }
}