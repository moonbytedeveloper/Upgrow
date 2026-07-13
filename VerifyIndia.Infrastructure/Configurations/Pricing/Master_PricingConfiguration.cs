using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Configurations.Pricing
{
    public class Master_PricingConfiguration : IEntityTypeConfiguration<Master_Pricing>
    {
        public void Configure(EntityTypeBuilder<Master_Pricing> builder)
        {
            builder.ToTable("Master_Pricing");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("numeric(18,0)")
                .UseIdentityColumn()
                .IsRequired();


        }
    }
}
