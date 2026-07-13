using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Infrastructure.Configurations.WL
{
    /// <summary>
    /// EF Core Configuration for WL_ActivityLogs entity
    /// Handles table mapping, column constraints, indexes, and relationships
    /// </summary>
    public class WL_ActivityLogsConfiguration : IEntityTypeConfiguration<WL_ActivityLogs>
    {
        public void Configure(EntityTypeBuilder<WL_ActivityLogs> builder)
        {
            
            builder.ToTable("WL_ActivityLogs");

            
            builder.HasKey(x => x.Id);

            
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();           
         
            // CurrentHash - SHA256 hash of payload for integrity verification
            builder.Property(x => x.CurrentHash)
                .HasColumnType("varbinary(MAX)")
                .IsRequired(false);

            // PreviousHash - SHA256 hash of previous log for chain integrity
            builder.Property(x => x.PreviousHash)
                .HasColumnType("varbinary(MAX)")
                .IsRequired(false);

            // DigitalSignature - RSA-PKCS1 signature for authenticity
            builder.Property(x => x.DigitalSignature)
                .HasColumnType("nvarchar(MAX)")
                .IsUnicode(true)
                .IsRequired(false);

            // PayLoad - JSON serialized activity data
            builder.Property(x => x.PayLoad)
                .HasColumnType("nvarchar(MAX)")
                .IsUnicode(true)
                .IsRequired(false);
          

            // TenantId - Multi-tenancy support
            builder.Property(x => x.TenantId)
                .HasColumnType("int")
                .IsRequired();

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_WL_ActivityLogs_TenantId");

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever(); // Explicit value in INSERT
        }
    }
}