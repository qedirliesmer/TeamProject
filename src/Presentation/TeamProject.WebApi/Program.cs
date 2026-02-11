using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.Validations.CityValidations;
using TeamProject.Application.Validations.DistrictValidations;
using TeamProject.Infrastructure.Extensions;
using TeamProject.Infrastructure.Services;
using TeamProject.Persistence.Contexts;
using TeamProject.Persistence.Repositories;
using TeamProject.Persistence.Services;
using TeamProject.Persistence.UnitOfWorks;
using TeamProject.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
