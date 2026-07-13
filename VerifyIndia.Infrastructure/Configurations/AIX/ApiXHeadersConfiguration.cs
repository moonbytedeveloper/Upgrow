using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations.AIX
{
    public class ApiXHeadersConfiguration : IEntityTypeConfiguration<ApiXHeaders>
    {
        public void Configure(EntityTypeBuilder<ApiXHeaders> builder)
        {
            // Table name
            builder.ToTable("ApiXHeaders");

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
                .HasDatabaseName("IX_ApiXHeaders_UUID");

            // CategoryName
            builder.Property(x => x.FieldName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasIndex(x => x.ApiXVersionUUID)
                .HasDatabaseName("IX_ApiXHeaders_ApiXVersionUUID");

            builder.Property(x => x.IsActive)
               .IsRequired()
               .HasDefaultValue(true)
               .ValueGeneratedNever();

            // IsActive - Active status flag

        }
    }
}
    

