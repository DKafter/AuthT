using AuthExample.Application.Data.Dtos;
using AuthExample.Application.Services;
using AuthExample.Core.Abstraction.Errors;
using AuthExample.Core.Abstraction.Interfaces;
using AuthExample.Database.Data;
using AuthExample.Models.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString(nameof(AppDbContext)));
});
builder.Services.AddScoped<IErrorFactory, ErrorFactory>();
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods("http://localhost:3000").AllowAnyMethod();
    x.WithOrigins("https://localhost:3000");
    x.WithMethods("https://localhost:3000").AllowAnyMethod();
});

app.Run();
