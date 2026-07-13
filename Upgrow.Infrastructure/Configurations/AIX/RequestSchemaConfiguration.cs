using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations.AIX
{
    public class RequestSchemaConfiguration : IEntityTypeConfiguration<RequestSchema>
    {
        public void Configure(EntityTypeBuilder<RequestSchema> builder)
        {
            // Table name
            builder.ToTable("RequestSchema");

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
                .HasDatabaseName("IX_RequestSchema_UUID");

            // CategoryName
            builder.Property(x => x.RequestType)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasIndex(x => x.RequestJson)
                .HasDatabaseName("IX_RequestSchema_RequestJson");


            // IsActive - Active status flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();
        }
    }
}
    
