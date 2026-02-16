using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Options;
using TeamProject.Domain.Constants;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Data;

public static class AdminSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager, IOptions<SeedOptions> seedOptions)
    {
        var options = seedOptions.Value;

        if (string.IsNullOrEmpty(options.AdminEmail)) return;

        var adminUser = await userManager.FindByEmailAsync(options.AdminEmail);

        if (adminUser == null)
        {
            var admin = new User
            {
                UserName = options.AdminEmail,
                Email = options.AdminEmail,
                FullName = options.AdminFullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, options.AdminPassword!);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, RoleNames.Admin);
            }
        }
    }
}

