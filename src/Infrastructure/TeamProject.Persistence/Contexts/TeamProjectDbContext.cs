using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Contexts;

public class TeamProjectDbContext:DbContext
{
    public TeamProjectDbContext(DbContextOptions<TeamProjectDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeamProjectDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<PropertyAd> PropertyAds { get; set; }
    public DbSet<PropertyMedia> PropertyMedias { get; set; }

}

