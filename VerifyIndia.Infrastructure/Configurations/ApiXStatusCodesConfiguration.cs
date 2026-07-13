using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class ApiXStatusCodesConfiguration : IEntityTypeConfiguration<ApiXStatusCodes>
    {
        public void Configure(EntityTypeBuilder<ApiXStatusCodes> builder)
        {
            // Table name
            builder.ToTable("ApiXStatusCodes");

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
                .HasDatabaseName("IX_ApiXStatusCodes_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_ApiXStatusCodes_Title");

            // StatusCode
            builder.Property(x => x.StatusCode)
                .IsRequired();

            builder.HasIndex(x => x.StatusCode)
                .HasDatabaseName("IX_ApiXStatusCodes_StatusCode");

            // IsSuccess
            builder.Property(x => x.IsSuccess)
                .IsRequired()
                .HasDefaultValue(false);

            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}