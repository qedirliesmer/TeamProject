using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("Districts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired() 
            .HasMaxLength(100) 
            .HasColumnName("Name");

        builder.Property(x => x.CityId)
            .IsRequired(); 

        builder.HasIndex(x => new { x.Name, x.CityId })
            .IsUnique();

        builder.HasOne(d => d.City)
            .WithMany(c => c.Districts)
            .HasForeignKey(d => d.CityId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
