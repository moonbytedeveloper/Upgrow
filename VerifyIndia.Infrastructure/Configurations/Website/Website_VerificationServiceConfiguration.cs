using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerifyIndia.Domain.Entities;
 

namespace VerifyIndia.Infrastructure.Configurations.Website
{
    public class Website_VerificationServiceConfiguration : IEntityTypeConfiguration<Website_VerificationService>
    {
        public void Configure(EntityTypeBuilder<Website_VerificationService> builder)
        {
            // Table name
            builder.ToTable("Website_VerificationService");

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
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_Website_VerificationService_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_Website_VerificationService_Title");

            builder.Property(x => x.ServiceCategoryUUID)
             .HasMaxLength(50)
             .IsRequired(true);

            builder.HasIndex(x => x.ServiceCategoryUUID)
                .HasDatabaseName("IX_Website_VerificationService_ServiceCategoryUUID");


            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT


        }
    }
}
    