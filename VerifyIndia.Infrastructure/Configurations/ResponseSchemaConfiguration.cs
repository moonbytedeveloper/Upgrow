using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;

namespace Upgrow.Infrastructure.Configurations
{
    public class ResponseSchemaConfiguration : IEntityTypeConfiguration<ResponseSchema>
    {
        public void Configure(EntityTypeBuilder<ResponseSchema> builder)
        {
            // Table name
            builder.ToTable("ResponseSchema");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID - Unique identifier for external references
            builder.Property(x => x.ApiXVersionUUID)
                .HasMaxLength(36)
                .IsUnicode(false);

           
            builder.Property(x => x.ShortDescription)
                .HasMaxLength(50)
                .IsRequired(false);

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Tell EF Core to always include this in INSERT
                                        // Query filter for soft delete (optional - apply if you want automatic filtering)
        }
    }
}
