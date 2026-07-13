using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Table name
            builder.ToTable("News");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id - Auto-increment identity column
            builder.Property(x => x.Id)
                .HasColumnType("decimal(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            // UUID
            builder.Property(x => x.UUID)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasIndex(x => x.UUID)
                .IsUnique()
                .HasFilter("[UUID] IS NOT NULL")
                .HasDatabaseName("IX_News_UUID");

            // Title
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Title)
                .HasDatabaseName("IX_News_Title");

            // BlogCategoryUUID - Foreign key reference
            builder.Property(x => x.NewsCategoryUUID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(x => x.NewsCategoryUUID)
                .HasDatabaseName("IX_News_NewsCategoryUUID");

            // Banner Image
            builder.Property(x => x.Image)
                .HasColumnType("nvarchar(200)")
                .IsRequired(true);

            // Short Description
            builder.Property(x => x.ShortDescription)
                .HasColumnType("nvarchar(500)")
                .IsRequired(false);

            // Blog Date
            //builder.Property(x => x.CreatedAt)
            //    .HasColumnType("datetime")
            //    .IsRequired(false);

            //builder.HasIndex(x => x.CreatedAt)
            //    .HasDatabaseName("IX_News_CreatedAt");

            // Full Description
          

            // IsActive - Soft delete flag
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.Property(x => x.IsTopStory)
               .IsRequired()
               .HasDefaultValue(false)
               .ValueGeneratedNever(); 

        }
    }
}
   