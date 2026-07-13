using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations
{
    public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            // Table name
            builder.ToTable("AppSetting");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("bigint")
                .UseIdentityColumn()
                .IsRequired();

            // Title
            builder.Property(x => x.Key)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.HasIndex(x => x.Key)
                .HasDatabaseName("IX_AppSetting_Key");
            // Code - System identifier (auto-generated)

            builder.Property(x => x.Value)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasIndex(x => x.Value)
                .HasDatabaseName("IX_AppSetting_Value");

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasIndex(x => x.Description)
                .HasDatabaseName("IX_AppSetting_Description");



            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();  
        }
    }
}
    
   
