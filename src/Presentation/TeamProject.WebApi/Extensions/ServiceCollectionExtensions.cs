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

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName)); 

        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
