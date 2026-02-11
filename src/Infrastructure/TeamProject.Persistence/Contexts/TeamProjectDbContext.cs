using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Contexts;

public class TeamProjectDbContext : IdentityDbContext<User>
{
    public TeamProjectDbContext(DbContextOptions<TeamProjectDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeamProjectDbContext).Assembly);
    }

    public DbSet<PropertyAd> PropertyAds { get; set; }
    public DbSet<PropertyMedia> PropertyMedias { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }

}

