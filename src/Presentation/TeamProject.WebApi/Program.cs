using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.Options;
using TeamProject.Application.Validations.CityValidations;
using TeamProject.Application.Validations.DistrictValidations;
using TeamProject.Domain.Entities;
using TeamProject.Infrastructure.Extensions;
using TeamProject.Infrastructure.Services;
using TeamProject.Persistence.Contexts;
using TeamProject.Persistence.Data;
using TeamProject.Persistence.Repositories;
using TeamProject.Persistence.Services;
using TeamProject.Persistence.UnitOfWorks;
using TeamProject.WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();