using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Infrastructure.Configurations.CustomerPanel
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Upgrow.Domain.Entities.Auth;

    public class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            // Table Name
            builder.ToTable("RefreshTokens");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id (decimal identity)
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // CustomerUUID
            builder.Property(x => x.CustomerUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.CustomerUUID)
                .HasDatabaseName("IX_RefreshTokens_CustomerUUID");

            // RefreshToken
            builder.Property(x => x.RefreshToken)
                .HasColumnType("nvarchar(500)")
                .IsRequired(true);

            builder.HasIndex(x => x.RefreshToken)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_RefreshToken");

            // ExpiryAt
            builder.Property(x => x.ExpiryAt)
                .HasColumnType("datetimeoffset")
                .IsRequired(false);

            builder.HasIndex(x => x.ExpiryAt)
                .HasDatabaseName("IX_RefreshTokens_ExpiryAt");

            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetimeoffset")
                .IsRequired(false);

            // IsRevoked
            builder.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false)
                .ValueGeneratedNever();
        }
    }
}
