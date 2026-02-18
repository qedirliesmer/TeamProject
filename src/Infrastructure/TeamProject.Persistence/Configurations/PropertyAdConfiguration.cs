using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Configurations;

public class PropertyAdConfiguration : IEntityTypeConfiguration<PropertyAd>
{
    public void Configure(EntityTypeBuilder<PropertyAd> builder)
    {
        builder.Property(x => x.Title).IsRequired().HasMaxLength(250);

        builder.Property(x => x.Description).HasMaxLength(2000);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Property(x => x.OfferType).HasConversion<string>();
        builder.Property(x => x.PropertyCategory).HasConversion<string>();

        builder.HasMany(x => x.MediaItems)
               .WithOne(x => x.PropertyAd)
               .HasForeignKey(x => x.PropertyAdId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.User)
           .WithMany(x => x.PropertyAds)
           .HasForeignKey(x => x.UserId)
           .OnDelete(DeleteBehavior.Cascade); 
    }
}