using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Configurations;

public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
{ 
    public void Configure(EntityTypeBuilder<PropertyMedia> builder)
    {
        builder.ToTable("PropertyMedias");

        builder.Property(x => x.ObjectKey)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Order)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.PropertyAdId)
            .IsRequired();

        builder.HasOne(x => x.PropertyAd)
            .WithMany(x => x.MediaItems)
            .HasForeignKey(x => x.PropertyAdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.PropertyAdId); 
        builder.HasIndex(x => new { x.PropertyAdId, x.Order });
    }
}