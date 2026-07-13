using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Configurations
{
    public class Master_CustomerConfiguration : IEntityTypeConfiguration<Master_Customer>
    {
        public void Configure(EntityTypeBuilder<Master_Customer> builder)
        {
            builder.ToTable("Master_Customer");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.UUID)
                .HasMaxLength(50);

            builder.Property(x => x.FName)
                .HasColumnName("FName")
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(x => x.MName)
                .HasColumnName("MName")
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(x => x.LName)
                .HasColumnName("LName")
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(x => x.Mobile)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(x => x.ACType)
                .HasMaxLength(50)
                .IsRequired(false);         

            builder.Property(x => x.ACLink)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.TenantId)
                .HasColumnType("numeric(18,0)")
                .IsRequired(true);

            builder.Property(x => x.CityUUID)
                .HasMaxLength(50)
                .IsRequired(false);
            builder.Property(x => x.StateUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.IndustryUUID)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.ReferralCode)
                .HasMaxLength(20)
                .IsRequired(false);
            
            builder.Property(x => x.RegTimeStamp)
                .HasColumnType("datetimeoffset(7)")
                .IsRequired(false);

            builder.Property(x => x.XApiKey)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            builder.HasIndex(x => x.Mobile)
                .HasDatabaseName("IX_Master_Customer_Mobile");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Master_Customer_ACLinkId");
        }
    }
}
