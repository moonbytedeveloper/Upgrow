using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class PaymentGatewayCredentialConfiguration :IEntityTypeConfiguration<PaymentGatewayCredential>
    {
        public void Configure(EntityTypeBuilder<PaymentGatewayCredential> builder)
        {
            // Table name
            builder.ToTable("PaymentGatewayCredential");

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
                .HasDatabaseName("IX_PaymentGatewayCredential_UUID");

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT


            // Query filter for soft delete (optional - apply if you want automatic filtering)
            // builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}
