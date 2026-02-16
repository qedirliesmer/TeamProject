using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.DTOs.PropertyAdDTOs;
using TeamProject.Application.Options;
using TeamProject.Application.Validations.AuthValidations;
using TeamProject.Application.Validations.CityValidations;
using TeamProject.Application.Validations.DistrictValidations;
using TeamProject.Domain.Entities;
using TeamProject.Infrastructure.Extensions;
using TeamProject.Infrastructure.Services;
using TeamProject.Persistence.Contexts;
using TeamProject.Persistence.Repositories;
using TeamProject.Persistence.Services;
using TeamProject.Persistence.UnitOfWorks;
using TeamProject.WebApi.Options;
using Microsoft.OpenApi.Models;
using TeamProject.Domain.Constants;

namespace TeamProject.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinioStorage(configuration);
        services.AddScoped<IFileStorageService, S3MinioFileStorageService>();

        services.AddDbContext<TeamProjectDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(typeof(PropertyAdCreateDto).Assembly);

        services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();

        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IPropertyAdService, PropertyAdService>();
        services.AddScoped<IPropertyAdRepository, PropertyAdRepository>();
        services.AddScoped<IPropertyMediaRepository, PropertyMediaRepository>();

        services.AddValidatorsFromAssemblyContaining<DistrictCreateRequestDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CityCreateDtoValidator>();

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8; 
            options.User.RequireUniqueEmail = true; 
        })
        .AddEntityFrameworkStores<TeamProjectDbContext>() 
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });
        services.Configure<SeedOptions>(configuration.GetSection(SeedOptions.SectionName));

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ManageCities, p => p.RequireRole(RoleNames.Admin));

            options.AddPolicy(Policies.ManageProperties, p => p.RequireAuthenticatedUser());
        });


        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                  ?? throw new InvalidOperationException("JWT section is missing.");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
        });

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Token daxil edin"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
       
    }
}
