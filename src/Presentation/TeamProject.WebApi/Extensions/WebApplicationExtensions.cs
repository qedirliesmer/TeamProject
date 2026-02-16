using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TeamProject.Application.Options;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Data;

namespace TeamProject.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ConfigurePipelineAsync(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var seedOptions = services.GetRequiredService<IOptions<SeedOptions>>();

            await RoleSeeder.SeedAsync(roleManager);

            if (app.Environment.IsDevelopment())
            {
                await AdminSeeder.SeedAsync(userManager, seedOptions);
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Uygulama başlayanda seeding zamanı xəta baş verdi.");
        }
    }

    public static void ConfigurePipeline(this WebApplication app)
    {
        app.ConfigurePipelineAsync().GetAwaiter().GetResult();
    }
}
