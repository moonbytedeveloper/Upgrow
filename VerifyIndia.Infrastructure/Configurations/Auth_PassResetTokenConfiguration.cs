using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Auth_PassResetTokenConfiguration : IEntityTypeConfiguration<Auth_PassResetToken>
    {
        public void Configure(EntityTypeBuilder<Auth_PassResetToken> builder)
        {
            // Table name
            builder.ToTable("Auth_PassResetToken");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.Token)
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.HasIndex(x => x.Token)
                .IsUnique()
                .HasFilter("[Token] IS NOT NULL")
                .HasDatabaseName("IX_Auth_PassResetToken_Token");

            // MobileNo
            builder.Property(x => x.EmployeeUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsUsed)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
                                        // Query filter for soft delete (optional - apply if you want automatic filtering)
        }
    }
}
